using UnityEngine;

public class Player : MonoBehaviour
{
    private LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Restart level generation
            foreach (Transform child in levelGenerator.transform)
            {
                Destroy(child.gameObject);
            }
            levelGenerator.GenerateLevel();
            if (levelGenerator.transform.Find("Start").position != null)
            {
                transform.position = levelGenerator.transform.Find("Start").position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "End")
        {
            // Go to the next level
            foreach (Transform child in levelGenerator.transform)
            {
                Destroy(child.gameObject);
            }
            levelGenerator.GenerateLevel();
            transform.position = levelGenerator.transform.Find("Start").position;
        }
    }
}
