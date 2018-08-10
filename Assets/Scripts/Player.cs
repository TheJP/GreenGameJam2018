using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private Sprite sprite;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int maxOxygen;
    private int currentOxygen;
    //Phobie
    //Equipped Weapon
    //Inventory

    // Use this for initialization
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.spriteRenderer.sprite = sprite;

        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            this.animator.enabled = true;
        }
        else
        {
            this.animator.enabled = false;
        }
    }
}