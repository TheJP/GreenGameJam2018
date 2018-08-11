using UnityEngine;

public class CrosshairController : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField]
	private float crosshairSpeed = 2f;
#pragma warning restore 0649
	
	private SpriteRenderer crosshairRenderer;
	
	public Color CrosshairColor
	{
		get { return crosshairRenderer.color; }
		set { crosshairRenderer.color = value; }
	}
	
	public int PlayerIndex { get; set; }
	
	public bool HasValidHit { get; set; }

	private void Awake()
	{
		crosshairRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (PlayerIndex <= 0)
		{
			return;
		}
		
		var horizontal = Input.GetAxis($"Horizontal_{PlayerIndex}") * crosshairSpeed * Time.deltaTime;
		var vertical = Input.GetAxis($"Vertical_{PlayerIndex}") * crosshairSpeed * Time.deltaTime;
		
		var translation = new Vector3(horizontal, vertical, 0);
		
		transform.Translate(translation);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var spriteRenderer = other.GetComponent<SpriteRenderer>();
		if (spriteRenderer == null)
		{
			return;
		}

		this.HasValidHit = spriteRenderer.color == CrosshairColor;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		this.HasValidHit = false;
	}
}
