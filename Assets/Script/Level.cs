using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {
	public int sizeX, sizeY;

	public int scale;

	public Tile tilePrefab;

	public Tile[] allTiles;
	public Tile[,] tileMap;

	public void Initialize(){
		tileMap = new Tile[sizeX, sizeY];
		allTiles = new Tile[sizeX * sizeY];

		int i = 0;
		for(int x = 0; x < sizeX; x++){
			for(int y = 0; y < sizeY; y++){
				Tile tile = tileMap[x, y] = allTiles[i] = Instantiate(tilePrefab, new Vector3(x * scale, y * scale, 0), Quaternion.identity) as Tile;
				tile.tileType = Random.value > 0.10 ? TileType.Ground : TileType.Rock;

				tile.gameObject.name = x + ", " + y;
				tile.transform.parent = transform;
				tile.transform.localScale = new Vector3(scale, scale, 1);
				i++;
			}
		}

		for(int x = 0; x < sizeX; x++){
			for(int y = 0; y < sizeY; y++){
				Tile tile = tileMap[x, y];
				tile.neighbors = new List<Tile>();
				if(x > 0){
					tile.neighbors.Add(tileMap[x - 1, y]);
				}
				if(x < sizeX - 1){
					tile.neighbors.Add(tileMap[x + 1, y]);
				}
				if(y > 0){
					tile.neighbors.Add(tileMap[x, y - 1]);
				}
				if(y < sizeY - 1){
					tile.neighbors.Add(tileMap[x, y + 1]);
				}
			}
		}


	}


}
