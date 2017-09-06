using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundGeneration : MonoBehaviour
{
    [System.Serializable]
    class DecorationInfos
    {
        public GameObject decoration = null;
        public Vector2 size = new Vector2(1, 1);
    }

    [System.Serializable]
    class RegularDecorationInfos
    {
        public GameObject decoration = null;
        public int pos = 0;
        public int repeat = 0;
        public int timer = 0;
    }

    [System.Serializable]
    class WallInfos
    {
        public GameObject rightWall = null;
        public int rightWidth = 0;
        public GameObject leftWall = null;
        public int leftWidth = 0;
        public GameObject filler = null;
    }

    [System.Serializable]
    class PathInfos
    {
        public GameObject pathRight = null;
        public GameObject pathLeft = null;
        public GameObject filler = null;
        public int width = 0;
    }

    [System.Serializable]
    class LevelInfos
    {
        public List<GameObject> tiles = new List<GameObject>();
        public int probability = 0;
        public int decorationZone = 0;
        public List<DecorationInfos> decorations = new List<DecorationInfos>();
        public List<RegularDecorationInfos> regularDecorations = new List<RegularDecorationInfos>();
        public PathInfos path = new PathInfos();
        public WallInfos walls = new WallInfos();
    }

    [SerializeField]
    private int level;
    [SerializeField]
    private float speed;
    [SerializeField]
    private List<LevelInfos> levels;
    [SerializeField]
    private List<int> levelBinding;

    [SerializeField]
    private Vector2 fieldSize;

    private Vector2 size;
    private Vector2 movement;
    private List<int> decorationCD = new List<int>();

    private float moved;

    [SerializeField]
    private bool paused = false;
    [SerializeField]
    private bool mainMenu = false;

	// Use this for initialization
	void Start ()
    {
        level = levelBinding[level];
        // Si ApplicationDatas.Level vaut -1, on ne vient pas de l'overworld donc on est en debug
        if (!mainMenu && ApplicationDatas.Game.CurrentLevel >= 0)
        {
            int tmp = ApplicationDatas.Game.CurrentLevel;
            if (tmp >= levelBinding.Count)
            {
                tmp = 0;
            }
            level = levelBinding[tmp];
        }
        moved = 0;
        movement = new Vector2(0, -speed);
        size.x = levels[0].tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * levels[0].tiles[0].transform.localScale.x - 0.01f;
        size.y = levels[0].tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y * levels[0].tiles[0].transform.localScale.y - 0.01f;

        for (int i = 0; i < fieldSize.x; ++i)
        {
            decorationCD.Add(0);
        }

        Vector2 pos = new Vector2(0, transform.position.y - (fieldSize.y - 2) * size.y / 2);
        for (int i = 0; i < fieldSize.y; ++i)
        {
            DecrementDecorationCooldown();
            pos.x = transform.position.x - (fieldSize.x - 1) * size.x / 2;
            for (int j = 0; j < fieldSize.x; ++j)
            {
                GenerateTile(j, pos);
                pos.x += size.x;
            }
            for (int k = 0; k < levels[level].regularDecorations.Count; ++k)
            {
                levels[level].regularDecorations[k].timer -= 1;
            }
            pos.y += size.y;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (paused) return;

        Vector2 mv = movement * Time.deltaTime;
        moved += Mathf.Abs(mv.y);
        foreach (Transform child in transform)
        {
            child.Translate(mv);
        }
        while (moved > size.y)
        {
            moved -= size.y;
            DecrementDecorationCooldown();
            Vector2 pos = new Vector2(transform.position.x - (fieldSize.x - 1) * size.x / 2, transform.GetChild(0).transform.position.y + size.y);
            for (int i = 0; i < fieldSize.x; ++i)
            {
                GenerateTile(i, pos);
                GameObject.Destroy(transform.GetChild(transform.childCount - 1 - i).gameObject);
                pos.x += size.x;
            }
            for (int k = 0; k < levels[level].regularDecorations.Count; ++k)
            {
                levels[level].regularDecorations[k].timer -= 1;
            }
        }
    }

    private void DecrementDecorationCooldown()
    {
        for (int i = 0; i < decorationCD.Count; ++i)
        {
            if (decorationCD[i] > 0)
            {
                --decorationCD[i];
            }
        }
    }

    private void GenerateTile(int index, Vector2 position)
    {
        GameObject t;

        if (levels[level].walls.leftWall != null && index <= levels[level].walls.leftWidth)
        {
            if (index == levels[level].walls.leftWidth)
            {
                t = Instantiate(levels[level].walls.leftWall, position, transform.rotation) as GameObject;
            }
            else
            {
                t = Instantiate(levels[level].walls.filler, position, transform.rotation) as GameObject;
            }
        }
        else if (levels[level].walls.rightWall != null && index >= fieldSize.x - 1 - levels[level].walls.rightWidth)
        {
            if (index == fieldSize.x - 1 - levels[level].walls.rightWidth)
            {
                t = Instantiate(levels[level].walls.rightWall, position, transform.rotation) as GameObject;
            }
            else
            {
                t = Instantiate(levels[level].walls.filler, position, transform.rotation) as GameObject;
            }
        }
        else
        {
            t = Instantiate(levels[level].tiles[Random.Range(0, levels[level].tiles.Count)], position, transform.rotation) as GameObject;
        }
        t.transform.parent = this.transform;
        t.transform.SetSiblingIndex(0);

        GeneratePath(index, position, t);
        for (int i = 0; i < levels[level].regularDecorations.Count; ++i)
        {
            if (levels[level].regularDecorations[i].pos == index &&
                levels[level].regularDecorations[i].timer <= 0)
            {
                GameObject deco = Instantiate(levels[level].regularDecorations[i].decoration, position, transform.rotation) as GameObject;
                deco.transform.parent = t.transform;
                levels[level].regularDecorations[i].timer = levels[level].regularDecorations[i].repeat;
                return;
            }
        }

        if (levels[level].decorations.Count > 0 && decorationCD[index] == 0 &&
            (position.x < transform.position.x - levels[level].decorationZone || position.x > transform.position.x + levels[level].decorationZone) &&
            Random.Range(0, levels[level].probability) == 0)
        {
            int d = Random.Range(0, levels[level].decorations.Count);
            GameObject deco = Instantiate(levels[level].decorations[d].decoration, position, transform.rotation) as GameObject;
            deco.transform.parent = t.transform;
            for (int i = 0; i < levels[level].decorations[d].size.x; ++i)
            {
                if (index + i < decorationCD.Count)
                {
                    decorationCD[index + i] = (int)levels[level].decorations[d].size.y;
                }
            }
        }
    }

    public void GeneratePath(int index, Vector2 position, GameObject parent)
    {
        if (levels[level].path.pathRight == null) return;

        GameObject path = null;
        int pathMin = (int)fieldSize.x / 2 - levels[level].path.width / 2;
        int pathMax = (int)fieldSize.x / 2 + levels[level].path.width / 2;

        if (pathMin == index)
        {
            path = Instantiate(levels[level].path.pathLeft, position, transform.rotation) as GameObject;
        }
        else if (pathMax == index)
        {
            path = Instantiate(levels[level].path.pathRight, position, transform.rotation) as GameObject;
        }
        else if (index <= pathMax && index >= pathMin)
        {
            path = Instantiate(levels[level].path.filler, position, transform.rotation) as GameObject;
        }
        if (path)
        {
            path.transform.parent = parent.transform;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Resume()
    {
        paused = false;
    }
}
