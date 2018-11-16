using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

    [Header("Set Dynamically")]
    public string suit;
    public int rank;
    public Color color = Color.black;
    public string colS = "Black";  

    public List<GameObject> decoGOs = new List<GameObject>();
    public List<GameObject> pipGOs = new List<GameObject>();

    public GameObject back;  
    public CardDefinition def;  		

    
    public SpriteRenderer[] spriteRenderers;

    void Start()
    {
        SetSortOrder(0); 
    }

    public void PopulateSpriteRenderers()
    {
        
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    void Start () {
	
	}

	void Update () {
	
	}
} 

[System.Serializable]
public class Decorator{
	public string	type;			
	public Vector3	loc;			
	public bool		flip = false;	
	public float 	scale = 1.0f;
}

[System.Serializable]
public class CardDefinition{
	public string	face;	
	public int		rank;	
	public List<Decorator>	
					pips = new List<Decorator>();  
}
