using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager manager;
	public static float completionTime;

	public Sprite[] sprites;

	public Level levelPrefab;

	public TileType nextTileType;
	public TileType[] plantables;
	public List<TileType> groundTypes;

	public AudioSource harvest, plant, plantBonus;

	public Image nextSeed;
	public Text foodText;

	private int totalHarvested;
	public int food{
		get{
			return totalHarvested;
		}
		set{
			totalHarvested = value;
			foodText.text = "Harvested: " + totalHarvested;
		}
	}

	private Level levelInstance;

	// Use this for initialization
	void Start () {
		manager = this;
		levelInstance = Instantiate(levelPrefab) as Level;
		levelInstance.Initialize();	

		nextTileType = plantables[Random.Range(0, plantables.Length)];
		nextSeed.sprite = sprites[(int)nextTileType];
		completionTime = 0f;
		StartCoroutine(FoodTimer());
	}
		
	void Update(){
		completionTime += Time.deltaTime;
		Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, Mathf.Infinity)){
			Tile hitTile = hit.transform.gameObject.GetComponent<Tile>();

			if(hitTile != null){
				hitTile.highlighted = true;
			}

			if(Input.GetButtonDown("Interact")){
				if(groundTypes.Contains(hitTile.tileType)){
					
					hitTile.bonus = false;
					if(nextTileType == TileType.Corn && hitTile.tileType == TileType.CornGround){
						plantBonus.Play();
						hitTile.bonus = true;
					}
					else if(nextTileType == TileType.Potato && hitTile.tileType == TileType.PotatoGround){
						plantBonus.Play();
						hitTile.bonus = true;
					}
					else if(nextTileType == TileType.Carrot && hitTile.tileType == TileType.CarrotGround){
						plantBonus.Play();
						hitTile.bonus = true;
					}
					else if(nextTileType == TileType.Tomato && hitTile.tileType == TileType.TomatoGround){
						plantBonus.Play();
						hitTile.bonus = true;
					}
					else{
						plant.Play();
					}

					hitTile.Plant(nextTileType);

					nextTileType = plantables[Random.Range(0, plantables.Length)];
					nextSeed.sprite = sprites[(int)nextTileType];
					UpdateTiles();
				}
				else if(hitTile.harvestable){
					harvest.Play();
					hitTile.Harvest(1);
					UpdateTiles();
				}

				
			}
		}
	}

	void UpdateTiles(){
		foreach(Tile t in levelInstance.allTiles){
			t.UpdateTile();
		}
	}

	IEnumerator FoodTimer(){
		bool playing = true;
		food = 50;
		while(playing){

			if(food >= 100 || food <= 0){
				break;
			}

			food -= 1;
			yield return new WaitForSeconds(1f); 
		}

		if(food >= 100){
			Win();
		}
		else if(food <= 0){
			Lose();
		}

	}

	public void Win(){
		Application.LoadLevel("Win");
	}
	public void Lose(){
		Application.LoadLevel("Lose");
	}

}
