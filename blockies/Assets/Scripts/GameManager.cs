using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public List<List<List<int>>> WorldList;
    public List<Block> BlockList;

    GameObject BlockPrefab;

    int WorldSize;
    int BlockCount;
    int RulesPerType;
    int TypeCount;

    public List<Rule> RuleList;

    public class Rule
    {
        public Vector3 ActivatorLocation;
        public Vector3 EffectedLocationA; //can be 0,0,0 which is self
        public Vector3 EffectedLocationB;

        public int ActionCode;
        //0 - Move A to B if A not empty and B is empty (and children if possible and required)
        //1 - Swap A and B (and children if possible and required)
        //2 - Change A to B's Type
        //3 - Make A my parent if not myself
        //4 - Make A parentless
        //5 - Make A childless

        public int ActivatorTypeRequirement;
        public int EffectedTypeRequirement; //not required
        public bool BreakEffectedFromParent;
        public bool BreakEffectedFromChildren;
        public bool EffectedTypeMatchRequired;
    }

    public class Block
    {
        public int Type;
        public GameObject GO;
        public Vector3 Position;
        public bool HasParent;
        public Vector3 ParentPosition;
        public List<Vector3> ChildrenPositionList;
                
        public Block()
        {
            HasParent = false;
            ChildrenPositionList = new List<Vector3>();
            Position = Vector3.zero;
        }

        public void Update()
        {
            foreach(Rule cRule in G.GM.RuleList[Type])
            {
                switch(cRule.ActionCode)
                {
                    case 0:
                        Vector3 wActivatorPosition = G.GM.WrapPosition(new Vector3(Position.x + cRule.ActivatorLocation.x, Position.y + cRule.ActivatorLocation.y, Position.z + cRule.ActivatorLocation.z ));
                        if(G.GM.BlockList[G.GM.WorldList[wActivatorPosition.x][wActivatorPosition.y][wActivatorPosition.z]].Type == cRule.ActivatorTypeRequirement)
                        {
                            Vector3 wEffectedPosition = G.GM.WrapPosition(new Vector3(Position.x + cRule.EffectedLocationA.x, Position.y + cRule.EffectedLocationA.y, Position.z + cRule.EffectedLocationA.z));

                            if (!cRule.EffectedTypeMatchRequired || G.GM.BlockList[G.GM.WorldList[wEffectedPosition.x][wEffectedPosition.y][wEffectedPosition.z]].type == cRule.EffectedTypeRequirement)
                            {
                                int w2 = 0;
                            }
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    default:
                        break;
                }
            }
        }
    }

	// Use this for initialization
	void Start () {
        G.GM = this;

        WorldSize = 100;
        BlockCount = 1000;
        RulesPerType = 3;
        TypeCount = 5;

        RuleList = new List<Rule>();
        WorldList = new List<List<List<int>>>();
        BlockList = new List<Block>();

        for(int x = 0; x < WorldSize; x++)
        {
            List<List<int>> cBlockWall = new List<List<int>>();
            for(int y = 0; y < WorldSize; y++)
            {
                List<int> cBlockRow = new List<int>();
                for(int z = 0; z < WorldSize; z++)
                {
                    int Index = -1;
                    cBlockRow.Add(Index);
                }
                cBlockWall.Add(cBlockRow);
            }
            WorldList.Add(cBlockWall);
        }

        for(int i = 0; i < TypeCount; i++)
        {
            for(int j =0; j < RulesPerType; j++)
            {
                Rule cRule = new Rule();
                cRule.ActivatorLocation = new Vector3(Random.Range(-1,2),Random.Range(-1,2),Random.Range(-1,2));
                cRule.EffectedLocationA = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2)); //can be 0,0,0 which is self
                cRule.EffectedLocationB = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), Random.Range(-1, 2));

                cRule.ActionCode = Random.Range(0, 6);
                cRule.ActivatorTypeRequirement = Random.Range(0,TypeCount);
                cRule.EffectedTypeRequirement = Random.Range(0, TypeCount);//not required
                System.Boolean.TryParse(Random.Range(0,2).ToString(),out cRule.BreakEffectedFromParent);
                System.Boolean.TryParse(Random.Range(0, 2).ToString(), out cRule.BreakEffectedFromChildren);
                System.Boolean.TryParse(Random.Range(0, 2).ToString(), out cRule.EffectedTypeMatchRequired);
            }
        }

        for(int q = 0; q < BlockCount; q++)
        {
            Block cBlock = new Block();
            cBlock.GO = (GameObject)Instantiate(BlockPrefab);
            cBlock.Type = Random.Range(0, TypeCount);

            bool ValidPosition = false;

            do
            {
                Vector3 nPosition = new Vector3(Random.Range(0, WorldSize), Random.Range(0, WorldSize), Random.Range(0, WorldSize));
                if(WorldList[(int)nPosition.x][(int)nPosition.y][(int)nPosition.z] == -1)
                {
                    cBlock.Position = nPosition;
                    WorldList[(int)nPosition.x][(int)nPosition.y][(int)nPosition.z] = q;
                    ValidPosition = true;

                }

            } while (!ValidPosition);

            BlockList.Add(cBlock);

        }
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Block cBlock in BlockList)
        {
            cBlock.Update();
        }
	}

    public Vector3 WrapPosition(Vector3 dirtyVector)
    {
        Vector3 cleanVector;
        float x;
        float y;
        float z;

        if (dirtyVector.x < 0)
        {
            x += WorldSize;
        }
        if (dirtyVector.y < 0)
        {
            y += WorldSize;
        }
        if (dirtyVector.x < 0)
        {
            z += WorldSize;
        }
        if (dirtyVector.x >= WorldSize)
        {
            x -= WorldSize;
        }
        if (dirtyVector.y >= WorldSize)
        {
            y -= WorldSize;
        }
        if (dirtyVector.x >= WorldSize)
        {
            z -= WorldSize;
        }

        cleanVector = new Vector3(x, y, z);
        return cleanVector;
    }
}
