using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileType{
	Ground, PotatoGround, CarrotGround, TomatoGround, CornGround, Growing1, Growing2, Potato, Corn, Carrot, Tomato, Rock
}


[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour {
	public Sprite caution, selected, rotted;

	[Range(0f, 1f)]
	public float dangerChance = 0.05f;

	[Range(0f, 1f)]
	public float rotChance = 0.01f;

	[Range(0f, 1f)]
	public float growChance = 0.5f;

	public int minHarvest, maxHarvest;

	public bool highlighted;
	public int growthStage;

	public SpriteRenderer rend;
	public SpriteRenderer highlight;

	private TileType seedType;
	public bool growing, harvestable, danger, rotten;
	public bool bonus;

	public List<Tile> neighbors;

	private TileType type;
	public TileType tileType{
		get{
			return type;
		}
		set{
			type = value;
			rend.sprite = GameManager.manager.sprites[(int)type];
		}
	}


	void Update(){
		if(highlighted){
			highlight.sprite = selected;
			highlighted = false;
		}
		else if(danger){
			highlight.sprite = caution;
		}
		else if(rotten){
			highlight.sprite = rotted;
		}
		else{
			highlight.sprite = null;
		}
	}

	public void Plant(TileType seed){
		seedType = seed;

		growthStage = 0;
		tileType = TileType.Growing1;
		growing = true;
		danger = harvestable = rotten = false;
	}

	public void Harvest(int chain){
		if(seedType == TileType.Corn){
			tileType = TileType.PotatoGround;
		}
		else if(seedType == TileType.Potato){
			tileType = TileType.CarrotGround;
		}
		else if(seedType == TileType.Carrot){
			tileType = TileType.TomatoGround;
		}
		else if(seedType == TileType.Tomato){
			tileType = TileType.CornGround;
		}
		
		harvestable = false;
		danger = false;

		GameManager.manager.food += (Random.Range(minHarvest, maxHarvest) * chain) * (bonus ? 2 : 1);

		foreach(Tile n in neighbors){
			if(n.tileType == seedType && n.harvestable){
				n.Harvest(chain + 1);
			}
		}

	}

	public void UpdateTile(){
		if(growing){
			
			if(Random.value < growChance){
				growthStage++;
			}
			
			if(growthStage == 1){
				tileType = TileType.Growing1;
			}
			else if(growthStage == 2){
				tileType = TileType.Growing2;
			}
			else if(growthStage > 2){
				tileType = seedType;
				growing = false;
				harvestable = true;
			}
		}
		if(harvestable){
			if(!danger && Random.value < dangerChance){
				danger = true;
			}
			
			else if(danger && Random.value < rotChance){
				harvestable = false;
				danger = false;
				rotten = true;
			}
		}
	}

}
