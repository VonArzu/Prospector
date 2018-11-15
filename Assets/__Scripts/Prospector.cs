using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Prospector : MonoBehaviour {

	static public Prospector 	S;


    [Header("Set in Inspector")]
    public TextAsset deckXML;
    public TextAsset layoutXML;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Vector3 layoutCenter;
    public Vector2 fsPosMid = new Vector2(0.5f, 0.90f);
    public Vector2 fsPosRun = new Vector2(0.5f, 0.75f);
    public Vector2 fsPosMid2 = new Vector2(0.4f, 1.0f);
    public Vector2 fsPosEnd = new Vector2(0.5f, 0.95f);

    [Header("Set Dynamically")]
    public Deck deck;
    public Layout layout;
    public List<CardProspector> drawPile;
    public Transform layoutAnchor;
    public CardProspector target;
    public List<CardProspector> tableau;
    public List<CardProspector> discardPile;
    public FloatingScore fsRun;
    void Awake()
    {
        S = this;
    }

    void Start()
    {
        Scoreboard.S.score = ScoreManager.SCORE;
        deck = GetComponent<Deck>();
        deck.InitDeck(deckXML.text);
        Deck.Shuffle(ref deck.cards); 
        /*Card c;
		for (int cNum = 0; cNum < deck.cards.Count; cNum++) {
			c = deck.cards [cNum];
			c.transform.localPosition = new Vector3 ((cNum % 13) * 3, cNum / 13 * 4, 0);
		}*/

        layout = GetComponent<Layout>(); 
        layout.ReadLayout(layoutXML.text);
        drawPile = ConvertListCardsToListCardProspectors(deck.cards);
        LayoutGame();
    }
    CardProspector Draw()
    {
        CardProspector cd = drawPile[0]; 
        drawPile.RemoveAt(0); 
        return (cd); 
    }

    
    void LayoutGame()
    {
        
        if (layoutAnchor == null)
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
         
            layoutAnchor = tGO.transform; 
            layoutAnchor.transform.position = layoutCenter; 
        }
        CardProspector cp;
       
        foreach (SlotDef tSD in layout.slotDefs)
        {
            
            cp = Draw();
            cp.faceUp = tSD.faceUp;
            cp.transform.parent = layoutAnchor;
            cp.transform.localPosition = new Vector3(layout.multiplier.x * tSD.x,
                layout.multiplier.y * tSD.y, -tSD.layerID);
            cp.layoutID = tSD.id;
            cp.slotDef = tSD;
            cp.state = eCardState.tableau;
           
            cp.SetSortingLayerName(tSD.layerName); 
            tableau.Add(cp);
        }

        foreach (CardProspector tCP in tableau)
        {
            foreach (int hid in tCP.slotDef.hiddenBy)
            {
                cp = FindCardByLayoutID(hid);
                tCP.hiddenBy.Add(cp);
            }
        }

     
        MoveToTarget(Draw());
        
        UpdateDrawPile();
    }

    
    CardProspector FindCardByLayoutID(int layoutID)
    {
        foreach (CardProspector tCP in tableau)
        {
          
            if (tCP.layoutID == layoutID)
            {
          
                return (tCP);
            }
        }
    
        return (null);
    }

   
    void SetTableauFaces()
    {
        foreach (CardProspector cd in tableau)
        {
            bool faceUp = true; 
            foreach (CardProspector cover in cd.hiddenBy)
            {
                
                if (cover.state == eCardState.tableau)
                {
                    faceUp = false; 
                }
            }
            cd.faceUp = faceUp; 
        }
    }
    public void CardClicked(CardProspector cd)
    {
        switch (cd.state)
        {
            case eCardState.target:
                break;
            case eCardState.drawpile:
                MoveToDiscard(target); 
                MoveToTarget(Draw()); 
                UpdateDrawPile(); 
                ScoreManager.EVENT(eScoreEvent.draw);
                FloatingScoreHandler(eScoreEvent.draw);
                break;
            case eCardState.tableau:
                
                bool validMatch = true;
                if (!cd.faceUp)
                {
               
                    validMatch = false;
                }
                if (!AdjacentRank(cd, target))
                {
                    
                    validMatch = false;
                }
                if (!validMatch)
                    return; 
                tableau.Remove(cd); 
                MoveToTarget(cd); 
                SetTableauFaces(); 
                ScoreManager.EVENT(eScoreEvent.mine);
                FloatingScoreHandler(eScoreEvent.mine);
                break;
        }
        
        CheckForGameOver();
    }

    void CheckForGameOver()
    {
        
        if (tableau.Count == 0)
        {
          
            GameOver(true);
            return;
        }
       
        if (drawPile.Count > 0)
        {
            return;
        }
   
        foreach (CardProspector cd in tableau)
        {
            if (AdjacentRank(cd, target))
            {
                
                return;
            }
        }
      
        GameOver(false);
    }

    void GameOver(bool won)
    {
        if (won)
        {
            
            ScoreManager.EVENT(eScoreEvent.gameWin);
            FloatingScoreHandler(eScoreEvent.gameWin);
        }
        else
        {
         
            ScoreManager.EVENT(eScoreEvent.gameLoss);
            FloatingScoreHandler(eScoreEvent.gameWin);
        }
       
        SceneManager.LoadScene("__Prospector_Scene_0");
    }
