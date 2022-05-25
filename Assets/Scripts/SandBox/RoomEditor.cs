using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class RoomEditor : MonoBehaviour
{
    public enum Selection { TEXTURE, TRAP, MOB, PORTAL, DECOR, DELETE };
    public enum Layer { GROUND, WALLS };
    private LoadRoom _roomLoader;
    private Layer _layer = Layer.WALLS;
    private Selection _selection = Selection.TEXTURE;
    [SerializeField] GameObject room;
    [SerializeField] GameObject traps;
    [SerializeField] GameObject mobs;
    [SerializeField] GameObject decors;
    [SerializeField] GameObject prefabDecor;
    [SerializeField] GameObject portals;
    [SerializeField] private InputField _name;
    [SerializeField] private Grid _grid;
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private Tile _defaultWall;
    [SerializeField] private List<Image> _imagesTexture;
    [SerializeField] private Image _imageTrap;
    [SerializeField] private Image _imageMob;
    [SerializeField] private ButtonBoss _buttonBoss;
    private Sprite _texture;
    private GameObject _trap;
    private GameObject _mob;
    private Sprite _spriteDecor;
    [SerializeField] private GameObject _portal;
    private Tile _tile;
    private int _roomID = 0;

    // Start is called before the first frame update
    void Start()
    {
        _roomLoader = GameObject.Find("Load").GetComponent<LoadRoom>();
        _tile = ScriptableObject.CreateInstance<Tile>();

        //CrossSceneInfos.CrossSceneInformation = "0";

        if (CrossSceneInfos.CrossSceneInformation == null) CrossSceneInfos.CrossSceneInformation = "0";

        _roomID = int.Parse(CrossSceneInfos.CrossSceneInformation);
        GameObject.Find("Load").GetComponent<LoadRoom>().activeRoom = _roomID;

        ChangeName();
    }

    void ChangeName()
    {
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        _name.text = player.donjon.rooms[_roomID].name;
    }

    public void SwitchLayer(string layer)
    {
        _layer = (Layer)System.Enum.Parse(typeof(Layer), layer);
    }

    public void SwitchSelection(string selection)
    {
        _selection = (Selection)System.Enum.Parse(typeof(Selection), selection);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            switch (_selection)
            {
                case Selection.TEXTURE:
                    PlaceTile();
                    break;
                case Selection.TRAP:
                    PlaceTrap();
                    break;
                case Selection.MOB:
                    PlaceMob();
                    break;
                case Selection.PORTAL:
                    PlacePortal();
                    break;
                case Selection.DECOR:
                    PlaceDecor();
                    break;
                case Selection.DELETE:
                    Delete();
                    break;
                default:
                    break;
            }

        }
    }

    void Delete()
    {
        Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        pos = _grid.WorldToCell(pos);
        TileBase tileWall = _walls.GetTile(pos);
        TileBase tileGround = _ground.GetTile(pos);

        if (IsThereAnyTrap(pos))
        {
            Trap[] childScripts = traps.GetComponentsInChildren<Trap>();

            for (int i = 0; i < childScripts.Length; i++)
            {
                Trap myChildScript = childScripts[i];

                if (myChildScript.transform.position.x - 0.5f == pos.x && myChildScript.transform.position.y - 0.5f == pos.y)
                {
                    Destroy(myChildScript.gameObject);
                    return;
                }
            }
        }
        else if (IsThereAnyMob(pos))
        {
            AIHealthManager[] childScripts = mobs.GetComponentsInChildren<AIHealthManager>();

            for (int i = 0; i < childScripts.Length; i++)
            {
                AIHealthManager myChildScript = childScripts[i];

                if ((int)myChildScript.transform.position.x == pos.x && (int)myChildScript.transform.position.y == pos.y)
                {
                    DestroyImmediate(myChildScript.gameObject);
                    _buttonBoss.Refresh();
                    return;
                }
            }
        }
        else if (tileWall)
        {
            _walls.SetTile(pos, null);
        }
        else if (tileGround)
        {
            _ground.SetTile(pos, null);
        }
    }

    GameObject PlaceGameObject(GameObject newObject, Transform parent)
    {
        Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        pos = _grid.WorldToCell(pos);
        TileBase tileWall = _walls.GetTile(pos);
        TileBase tileGround = _ground.GetTile(pos);

        if (tileWall != null || tileGround == null || IsThereAnyTrap(pos) || IsThereAnyMob(pos))
        {
            return null;
        }
        else
        {
            if (newObject.name == "Boss" && IsThereABoss())
            {
                Debug.Log("Boss");
                return null;
            }
            Debug.Log("Can place object");
            Vector3 posFloat = pos;
            posFloat.x += 0.5f;
            posFloat.y += 0.5f;
            return Instantiate(newObject, posFloat, Quaternion.identity, parent);
        }
    }

    bool IsThereABoss()
    {
        AIAbilityManager[] childScripts = mobs.GetComponentsInChildren<AIAbilityManager>();

        _buttonBoss.Refresh();

        for (int i = 0; i < childScripts.Length; i++)
        {
            AIAbilityManager myChildScript = childScripts[i];

            if (myChildScript.transform.name == "Boss")
            {
                return true;
            }
        }
        return false;
    }

    void PlaceDecor()
    {
        GameObject newDecor = PlaceGameObject(prefabDecor, decors.transform);

        if (newDecor != null)
        {
            _spriteDecor = _texture;

            newDecor.GetComponent<SpriteRenderer>().sprite = _spriteDecor;
            newDecor.name = _spriteDecor.name;
        }
    }

    void PlaceTrap()
    {
        PlaceGameObject(_trap, traps.transform);
    }

    void PlaceMob()
    {
        GameObject newMob = PlaceGameObject(_mob, mobs.transform);

        _buttonBoss.Refresh();

        if (newMob != null)
        {
            // Delete the clone from the name
            newMob.name = newMob.name.Replace("(Clone)", "");

            // Disable components to stop movements
            newMob.GetComponent<AIMovementManager>().enabled = false;
            newMob.GetComponent<AIAbilityManager>().enabled = false;
        }
    }

    void PlacePortal()
    {
        PlaceGameObject(_portal, portals.transform);
    }

    void PlaceTile()
    {
        Vector3Int pos = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        pos = _grid.WorldToCell(pos);
        Tilemap map = (_layer == Layer.WALLS) ? _walls : _ground;

        TileBase tile = map.GetTile(pos);

        if (IsThereAnyTrap(pos))
        {
            Debug.Log("Can't place a tile there");
        }
        else
        {
            map.SetTile(pos, _tile);
        }
    }

    public void ChangeCurrentTexture(Sprite texture)
    {
        _texture = texture;
        _tile.sprite = _texture;
        _tile.name = _texture.name;

        foreach (Image image in _imagesTexture)
        {
            image.sprite = _texture;
        }
    }

    public void ChangeCurrentTrap(GameObject trap, Sprite sprite)
    {
        _trap = trap;
        _imageTrap.sprite = sprite;
    }

    public void ChangeCurrentMob(GameObject mob, Sprite sprite)
    {
        _mob = mob;
        _imageMob.sprite = sprite;
    }

    bool IsThereAnyTrap(Vector3Int pos)
    {
        Trap[] childScripts = traps.GetComponentsInChildren<Trap>();

        for (int i = 0; i < childScripts.Length; i++)
        {
            Trap myChildScript = childScripts[i];

            if (myChildScript.transform.position.x - 0.5f == pos.x && myChildScript.transform.position.y - 0.5f == pos.y)
            {
                return true;
            }
        }

        return false;
    }

    bool IsThereAnyMob(Vector3Int pos)
    {
        AIHealthManager[] childScripts = mobs.GetComponentsInChildren<AIHealthManager>();

        for (int i = 0; i < childScripts.Length; i++)
        {
            AIHealthManager myChildScript = childScripts[i];

            if ((int)myChildScript.transform.position.x == pos.x && (int)myChildScript.transform.position.y == pos.y)
            {
                return true;
            }
        }

        return false;
    }
}
