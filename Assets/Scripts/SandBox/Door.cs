using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Components")]
    public Room room;
    public Door otherDoor = null;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource audioSource;

    [Header("Prefabs")]
    [SerializeField] GameObject wall;
    [SerializeField] GameObject floor;

    [Header("Variables")]
    List<GameObject> walls = new List<GameObject>();

    public void Open()
    {
        if (otherDoor != null)
        {
            Debug.Log("There is another door");
            otherDoor.DestroyDoor();

            StartCoroutine(SpawnPath(false));

            DestroyDoor();
        }
    }

    public void OpenInstant()
    {
        if (otherDoor != null)
        {
            otherDoor.DestroyDoor();

            Vector3 basePosition = transform.position;
            Vector3 finalPosition = otherDoor.transform.position;

            int x = basePosition.x < finalPosition.x ? 1 : basePosition.x == finalPosition.x ? 0 : -1;
            int y = basePosition.y < finalPosition.y ? 1 : basePosition.y == finalPosition.y ? 0 : -1;

            basePosition += new Vector3(x, y, 0); // Prevent first tile

            while (basePosition != finalPosition)
            {
                GameObject newPath = Instantiate(floor, basePosition, Quaternion.identity);

                walls.Add(newPath);
                walls.Add(Instantiate(wall, basePosition + new Vector3(y, x, 0), Quaternion.identity));
                walls.Add(Instantiate(wall, basePosition + new Vector3(-y, -x, 0), Quaternion.identity));

                basePosition += new Vector3(x, y, 0);
            }

            DestroyDoor();
        }
    }

    private IEnumerator SpawnPath(bool instant = true)
    {
        Vector3 basePosition = transform.position;
        Vector3 finalPosition = otherDoor.transform.position;

        int x = basePosition.x < finalPosition.x ? 1 : basePosition.x == finalPosition.x ? 0 : -1;
        int y = basePosition.y < finalPosition.y ? 1 : basePosition.y == finalPosition.y ? 0 : -1;

        basePosition += new Vector3(x, y, 0); // Prevent first tile

        while (basePosition != finalPosition)
        {
            GameObject newPath = Instantiate(floor, basePosition, Quaternion.identity);

            Instantiate(wall, basePosition + new Vector3(y, x, 0), Quaternion.identity);
            Instantiate(wall, basePosition + new Vector3(-y, -x, 0), Quaternion.identity);

            basePosition += new Vector3(x, y, 0);

            if (!instant)
            {
                audioSource.Stop();
                audioSource.Play();
            }

            yield return new WaitForSeconds(instant ? 0 : 0.3f);
        }
    }

    public void DestroyDoor()
    {
        otherDoor = null;
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;
    }

    public void Close()
    {
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;

        foreach (GameObject item in walls)
        {
            Destroy(item);
        }

        walls.Clear();
    }
}
