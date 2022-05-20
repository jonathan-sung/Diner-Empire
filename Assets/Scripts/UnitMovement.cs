using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Node
{
    public bool walkable;
    public float gCost;
    public float hCost;
    public Vector3Int start;
    public Vector3Int location;
    public Vector3Int target;
    public Node parent;
    public List<Vector3Int> neighbours;

    public Node(bool walkable, Vector3Int start, Vector3Int location, Vector3Int target, Node parent)
    {
        this.walkable = walkable;
        this.start = start;
        this.location = location;
        this.target = target;
        this.parent = parent;
        if (parent == null) this.parent = this;
        GenerateNeighbours();
    }

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void GenerateNeighbours()
    {
        neighbours = new List<Vector3Int>();
        Vector3Int direction = start - target;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            neighbours.Add(new Vector3Int(1, 0, 0) + location);
            neighbours.Add(new Vector3Int(-1, 0, 0) + location);
            neighbours.Add(new Vector3Int(0, -1, 0) + location);
            neighbours.Add(new Vector3Int(0, 1, 0) + location);
        }
        else
        {
            neighbours.Add(new Vector3Int(0, -1, 0) + location);
            neighbours.Add(new Vector3Int(0, 1, 0) + location);
            neighbours.Add(new Vector3Int(1, 0, 0) + location);
            neighbours.Add(new Vector3Int(-1, 0, 0) + location);
        }
        if (parent != null) neighbours.Remove(parent.location);
    }

    public float GetHybridCost(int y, int x)
    {
        if (y > x)
        {
            return Mathf.Sqrt(2 * x * x) + (y - x);
        }
        else {
            return Mathf.Sqrt(2 * y * y) + (x - y);
        }
    }

    public float GetManhattanCost(int y, int x)
    {
        return (x + y);
    }

    public float GetDiagonalCost(int y, int x)
    {
        return Mathf.Sqrt((x * x) + (y * y));
    }
}

public class UnitMovement : MonoBehaviour
{
    private Vector3 target;
    public bool move;

    public float speed;
    private Tilemap tilemap;
    private Rigidbody2D body;

    private List<Node> path;
    private int pathIndex;
    public bool pathActive;
    private int pathSearchCap; //this is how many tiles the algorithm will search to find a path to the target, if the cap is exceeded, the algorithm will stop

    private Unit u;
    private AIController ai;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        tilemap = GameObject.Find("Tilemap_Walls").GetComponent<Tilemap>();
        target = transform.position;
        move = false;
        path = new List<Node>();
        pathSearchCap = 500;
        u = GetComponent<Unit>();
        ai = GetComponent<AIController>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = u.movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (u != null && (u.selected || Input.GetKey(KeyCode.G)) && !(u is Customer)) //
        {
            if (Input.GetButtonDown("Fire2") && (!EventSystem.current.IsPointerOverGameObject() || (EventSystem.current.IsPointerOverGameObject() && (Bubble.overBubble))))
            {
                Vector3 tileSelection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
                tileSelection += new Vector3(-1, -1, 0);
                if (ai != null && ((ai.currentMenu == 2 || ai.currentMenu == 3 || ai.currentMenu == 9 || ai.currentMenu == 12 || ai.currentMenu == 14) || ai.instructionInProgress)) return;
                Vector3Int currentPosition = Vector3Int.RoundToInt(transform.position + new Vector3(-0.5f, -0.5f, 0));
                if (UnitActive() || IsWholeNumber(transform.position.y - Unit.activeOffset.y)) currentPosition += new Vector3Int(0, -1, 0);
                if (currentPosition == Vector3Int.CeilToInt(tileSelection)) return;
                FindPath(currentPosition, Vector3Int.CeilToInt(tileSelection));
            }
        }
        if (pathActive && path != null && pathIndex < path.Count)
        {
            target.x = path[pathIndex].location.x + 0.5f;
            target.y = path[pathIndex].location.y + 0.5f;
            move = true;
        }
        else
        {
            move = false;
            body.velocity = Vector2.zero;
        }
        Vector2 moveVector = target - transform.position;
        if (move)
        {
            Vector2 dir = moveVector.normalized;
            body.MovePosition(new Vector2((transform.position.x + dir.x * speed * Time.deltaTime), (transform.position.y + dir.y * speed * Time.deltaTime)));
        }
        if (move && (moveVector.magnitude <= 0.2))
        {
            if (pathIndex == (path.Count - 1))
            {
                body.velocity = Vector2.zero;
                body.MovePosition(target);
            }
            pathIndex++;
        }
        if (move)
        {
            Animation(moveVector);
        }
        else
        {
            if (animator != null) animator.SetInteger("Direction", 0);
        }
    }

    void DisplayLocationCursor(Vector3 location)
    {
        GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Location Cursor"));
        g.transform.position = location;
    }

    public void ControlledMove(Vector3 location)
    {
        if (!move)
        {
            Vector2 moveVector = location - transform.position;
            Vector2 dir = moveVector.normalized;
            body.MovePosition(new Vector2((transform.position.x + dir.x * speed * Time.deltaTime), (transform.position.y + dir.y * speed * Time.deltaTime)));
            if (moveVector.magnitude <= 0.2)
            {
                body.velocity = Vector2.zero;
                body.MovePosition(location);
            }
        }
    }

    bool IsWholeNumber(float a)
    {
        if (Mathf.Abs(a - Mathf.Floor(a)) <= 1 && Mathf.Abs(a - Mathf.Floor(a)) >= 0.9f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool UnitActive()
    {
        if (u != null)
        {
            return (u.cooking || u.chopping || u.seasoning);
        }
        else
        {
            return false;
        }
    }

    bool CustomerActive()
    {
        if (u is Customer)
        {
            Customer customer = u as Customer;
            return (customer.sitting != 0 || customer.eating != 0);
        }
        else
        {
            return false;
        }
    }

    void Animation(Vector2 v)
    {
        if (UnitActive() || CustomerActive())
        {
            if (animator != null) animator.SetInteger("Direction", -1);
            return;
        }
        if (v.magnitude == 0)
        {
            if (animator != null) animator.SetInteger("Direction", 0);
            return;
        }
        else if (v.magnitude >= 0.2)
        {
            int dir = 0;
            float bearing = Vector2.SignedAngle(Vector2.up, v);
            if (bearing >= 135)
            {
                dir = 3;
            }
            else if (bearing > 45)
            {
                dir = 2;
            }
            else if (bearing >= -45)
            {
                dir = 1;
            }
            else if (bearing > -135)
            {
                dir = 2;
            }
            else if (bearing >= -180)
            {
                dir = 3;
            }
            if (animator != null) animator.SetInteger("Direction", dir);
            if (v.x > 0.5f)
            {
                spriteRenderer.flipX = true;
            }
            else if (v.x < -0.5f)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    public void Move(Vector3 destination)
    {
        if (destination == new Vector3(0, 0, -1)) return;
        if (Vector3Int.RoundToInt(transform.position + new Vector3(-0.5f, -0.5f, 0)) == Vector3Int.FloorToInt(destination)) return;
        FindPath(Vector3Int.RoundToInt(transform.position + new Vector3(-0.5f, -0.5f, 0)), Vector3Int.FloorToInt(destination));
    }

    private void FindPath(Vector3Int start, Vector3Int destination)
    {
        Node startNode = new Node(CheckWalkable(start), start, start, destination, null); //create start node
        Node targetNode = new Node(CheckWalkable(destination), start, destination, destination, null); //create destination node
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        open.Add(startNode);
        Node current = startNode;
        int counter = 0;
        ResetPath();
        if (!targetNode.walkable)
        {
            float lowestGCost = 9999; //a large number
            Node lowestNode = targetNode;
            for (int i = 0; i < targetNode.neighbours.Count; i++)
            {
                Node n = new Node(CheckWalkable(targetNode.neighbours[i]), start, targetNode.neighbours[i], destination, null);
                if (n.walkable && n.gCost < lowestGCost)
                {
                    lowestNode = n;
                    lowestGCost = n.gCost;
                }
            }

            if (lowestGCost == 9999)
            {
                return;
            }
            else
            {
                targetNode = lowestNode;
            }
        }
        do
        {
            current = FindLowestCost(open);
            open.Remove(current);
            closed.Add(current);
            if (current.location == targetNode.location)
            {
                targetNode.parent = current.parent;
                RetracePath(startNode, targetNode);
                if (path.Count != 0) pathActive = true;
                if (u.selected) DisplayLocationCursor(targetNode.location + new Vector3(0.5f, 0.5f, 0));
                return;
            }
            for (int i = 0; i < current.neighbours.Count; i++)
            {
                Node neighbour = new Node(CheckWalkable(current.neighbours[i]), start, current.neighbours[i], destination, current);
                if (!neighbour.walkable || ContainsLocation(closed, neighbour.location) != -1) continue;
                float newMovementCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || ContainsLocation(open, neighbour.location) == -1)
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;
                    if (ContainsLocation(open, neighbour.location) == -1)
                    {
                        open.Add(neighbour);
                    }
                }
            }
            counter++;
        } while (counter <= pathSearchCap);
    }

    public void ResetPath()
    {
        pathActive = false;
        pathIndex = 0;
        path.Clear();
    }

    float GetDistance(Node nodeA, Node nodeB)
    {
        return Mathf.Abs(nodeA.location.y - nodeB.location.y) + Mathf.Abs(nodeA.location.x - nodeB.location.x);
    }

    private bool CheckWalkable(Vector3Int t)
    {
        if (tilemap.GetColliderType(t) == Tile.ColliderType.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Node FindLowestCost(List<Node> l)
    {
        int index = 0;
        float lowest = l[index].fCost;
        for (int i = 1; i < l.Count; i++)
        {
            if (l[i].fCost < lowest)
            {
                lowest = l[i].fCost;
                index = i;
            }
            else if (l[i].fCost == lowest)
            {
                if (l[i].hCost < l[index].hCost)
                {
                    index = i;
                }
                else if (l[i].hCost == l[index].hCost)
                {
                    if (l[i].gCost > l[index].gCost)
                    {
                        index = i;
                    }
                }
            }
        }
        Node lowestNode = l[index];
        return lowestNode;
    }

    private int ContainsLocation(List<Node> l, Vector3Int v)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].location == v) return i;
        }
        return -1;
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        path.Clear();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
    }

    public void UpdateSpeed()
    {
        speed = u.movementSpeed;
    }
}

