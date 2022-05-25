using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    public enum Form { Square, Circle };
    Vector2Int _size;
    Form _form = Form.Circle;
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private Sprite _spriteWall;
    [SerializeField] private Sprite _spriteGround;
    [SerializeField] private GameObject _traps;
    [SerializeField] private GameObject _portals;
    [SerializeField] private GameObject _mobs;
    private Tile _defaultGround;
    private Tile _defaultWall;

    void Awake()
    {
        _size.x = 4;
        _size.y = 4;

        ChangeTiles(_spriteGround, _spriteWall);
    }

    public void ChangeTiles(Sprite ground, Sprite wall)
    {
        _defaultGround = GetTileFromSprite(ground);
        _defaultWall = GetTileFromSprite(wall);
    }

    public Tile GetTileFromSprite(Sprite sprite)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        tile.name = sprite.name;

        return tile;
    }

    void Start()
    {
        //GenerateRoom();
    }

    void MoveCamera()
    {
        // Center Room in the middle of the camera

        GameObject.Find("Main Camera").transform.position = new Vector3(_size.x / 2.0f, _size.y / 2.0f, -10.0f);
    }

    // Don't try to optimize this function it will probably not work anymore
    void DestroyAllChild(GameObject parent)
    {
        Transform[] childs = new Transform[parent.transform.childCount];

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            childs[i] = parent.transform.GetChild(i);
        }

        foreach (Transform child in childs)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void GenerateRoom()
    {
        DestroyAllChild(_traps);
        DestroyAllChild(_mobs);
        DestroyAllChild(_portals);

        switch (_form)
        {
            case Form.Square:
                GenerateWallsSquare();
                break;
            case Form.Circle:
                GenerateWallsCircle();
                break;
            default:
                break;
        }

        MoveCamera();
    }

    float Distance(int x, int y, float ratio)
    {
        return (float)System.Math.Sqrt((System.Math.Pow(y * ratio, 2)) + System.Math.Pow(x, 2));
    }

    bool Filled(int x, int y, float radius, float ratio)
    {
        return Distance(x, y, ratio) <= radius;
    }

    bool FatFilled(int x, int y, float ratio, float radius)
    {
        return Filled(x, y, radius, ratio) && !(
                   Filled(x + 1, y, radius, ratio) &&
                   Filled(x - 1, y, radius, ratio) &&
                   Filled(x, y + 1, radius, ratio) &&
                   Filled(x, y - 1, radius, ratio) &&
                   Filled(x + 1, y + 1, radius, ratio) &&
                   Filled(x + 1, y - 1, radius, ratio) &&
                   Filled(x - 1, y - 1, radius, ratio) &&
                   Filled(x - 1, y + 1, radius, ratio)
                );
    }

    void GenerateWallsCircle()
    {
        float width_r = _size.x / 2;
        float height_r = _size.y / 2;
        float ratio = width_r / height_r;
        int maxblocks_x = 0;
        int maxblocks_y = 0;
        int index = 0;

        if ((width_r * 2) % 2 == 0)
        {
            maxblocks_x = (int)System.Math.Ceiling(width_r - .5) * 2 + 1;
        }
        else
        {
            maxblocks_x = (int)System.Math.Ceiling(width_r) * 2;
        }

        if ((height_r * 2) % 2 == 0)
        {
            maxblocks_y = (int)System.Math.Ceiling(height_r - .5) * 2 + 1;
        }
        else
        {
            maxblocks_y = (int)System.Math.Ceiling(height_r) * 2;
        }

        Vector3Int[] positions = new Vector3Int[_size.x * _size.y];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int y = -maxblocks_y / 2 + 1; y <= maxblocks_y / 2 - 1; y++)
        {
            for (int x = -maxblocks_x / 2 + 1; x <= maxblocks_x / 2 - 1; x++, index++)
            {
                if (FatFilled(x, y, ratio, width_r))
                {
                    positions[index] = new Vector3Int(x + (int)width_r, y + (int)width_r, 0);
                    tileArray[index] = _defaultWall;
                }
            }
        }

        // Empty then replace the tiles
        _walls.ClearAllTiles();
        _walls.SetTiles(positions, tileArray);

        GenerateGroundCircle((int)width_r, (int)width_r);
    }

    void GenerateGroundCircle(int offset_width, int offset_height)
    {
        float width_r = (_size.x - 1) / 2;
        float height_r = (_size.y - 1) / 2;
        float ratio = width_r / height_r;
        int maxblocks_x = 0;
        int maxblocks_y = 0;
        int index = 0;

        if ((width_r * 2) % 2 == 0)
        {
            maxblocks_x = (int)System.Math.Ceiling(width_r - .5) * 2 + 1;
        }
        else
        {
            maxblocks_x = (int)System.Math.Ceiling(width_r) * 2;
        }

        if ((height_r * 2) % 2 == 0)
        {
            maxblocks_y = (int)System.Math.Ceiling(height_r - .5) * 2 + 1;
        }
        else
        {
            maxblocks_y = (int)System.Math.Ceiling(height_r) * 2;
        }

        Vector3Int[] positions = new Vector3Int[(_size.x - 1) * (_size.y - 1)];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int y = -maxblocks_y / 2 + 1; y <= maxblocks_y / 2 - 1; y++)
        {
            for (int x = -maxblocks_x / 2 + 1; x <= maxblocks_x / 2 - 1; x++, index++)
            {
                if (Distance(x, y, ratio) <= width_r)
                {
                    positions[index] = new Vector3Int(x + offset_width, y + (int)offset_height, 0);
                    tileArray[index] = _defaultGround;
                }
            }
        }

        // Empty then replace the tiles
        _ground.ClearAllTiles();
        _ground.SetTiles(positions, tileArray);
    }

    void GenerateWallsSquare()
    {
        Vector3Int[] positions = new Vector3Int[_size.x * _size.y];
        TileBase[] tileArray = new TileBase[positions.Length];
        int index = 0;

        //Debug.Log(positions.Length);

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++, index++)
            {
                positions[index] = new Vector3Int(x, y, 0);
                if (x == 0 || y == 0 || x == _size.x - 1 || y == _size.y - 1)
                {
                    tileArray[index] = _defaultWall;
                }
                else
                {
                    tileArray[index] = null;
                }
            }
        }

        // Empty then replace the tiles
        _walls.ClearAllTiles();
        _walls.SetTiles(positions, tileArray);

        GenerateGroundSquare();
    }

    void GenerateGroundSquare()
    {
        Vector3Int[] positions = new Vector3Int[(_size.x - 1) * (_size.y - 1)];
        TileBase[] tileArray = new TileBase[positions.Length];
        int index = 0;

        //Debug.Log(positions.Length);

        for (int x = 0; x < _size.x - 1; x++)
        {
            for (int y = 0; y < _size.y - 1; y++, index++)
            {
                positions[index] = new Vector3Int(x, y, 0);
                tileArray[index] = _defaultGround;
            }
        }

        // Empty then replace the tiles
        _ground.ClearAllTiles();
        _ground.SetTiles(positions, tileArray);
    }

    public void RandomRoom()
    {
        ChangeForm(Random.Range(0, 2));
        ChangeHeigth(Random.Range(4, 25));
        ChangeWidth(Random.Range(4, 25));
    }

    public void ChangeForm(int form)
    {
        _form = (Form)form;
        GenerateRoom();
    }

    public void ChangeWidth(System.Single width)
    {
        _size.x = (int)width;
        GenerateRoom();
    }

    public void ChangeHeigth(System.Single heigth)
    {
        _size.y = (int)heigth;
        GenerateRoom();
    }

    public void SetRandomTiles(TexturePack pack)
    {
        Tile[] grounds = new Tile[pack.grounds.Length];
        Tile[] walls = new Tile[pack.walls.Length];

        for (int i = 0; i < pack.grounds.Length; i++)
        {
            grounds[i] = GetTileFromSprite(pack.grounds[i]);
        }

        for (int i = 0; i < pack.walls.Length; i++)
        {
            walls[i] = GetTileFromSprite(pack.walls[i]);
        }

        SetRandomTilesOnTileMap(grounds, _ground);
        SetRandomTilesOnTileMap(walls, _walls);

    }

    void SetRandomTilesOnTileMap(Tile[] tiles, Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null && Random.Range(0, 10) == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tiles[Random.Range(0, tiles.Length)]);
                }
            }
        }
    }
}
