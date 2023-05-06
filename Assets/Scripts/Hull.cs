using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Hull : MonoBehaviour {
	[SerializeField] private float Density = 1250f;
	private Vector3 _offset => (transform.up + transform.right) / 2;
	private Tilemap _tilemap;
	private Dictionary<Vector3Int, (TileBase tile, Matrix4x4 transform)> _tiles = new();
	private Dictionary<Type, List<Vector3Int>> _tilesByType = new();

	public int GetCount() => _tiles.Count;
	public int GetCount<T>() where T : TileBase => _tilesByType[typeof(T)].Count;

	public float Mass => _tiles.Count * Density;

	private void Awake() {
		_tilemap = GetComponent<Tilemap>();
		foreach(var (pos, tile) in GetTiles()) {
			var matrix = _tilemap.GetTransformMatrix(pos);
			_tiles.Add(pos, (tile, matrix));
			if(_tilesByType.TryGetValue(tile.GetType(), out var list))
				list.Add(pos);
			else
				_tilesByType.Add(tile.GetType(), new List<Vector3Int>() { pos });

			GameObject tileObject = _tilemap.GetInstantiatedObject(pos);
			if(tileObject) {
				tileObject.transform.rotation = matrix.rotation;
			}

		}
	}

	public void CenterLocally() {
		Vector3 center = Vector3.zero;
		foreach(var (pos, _) in GetTiles()) {
			center += pos;
		}

		transform.localPosition = (-center / _tiles.Count) - _offset;
	}

	public GameObject GetTileGameObject(Vector3Int position) => _tilemap.GetInstantiatedObject(position);
	public T GetTileGameObject<T>(Vector3Int position) => _tilemap.GetInstantiatedObject(position).GetComponent<T>();

	public Matrix4x4 GetTransform(Vector3Int position) => _tiles[position].transform;
	public Vector3 WorldToLocal(Vector3 worldPosition) => _tilemap.WorldToLocal(worldPosition);
	public Vector3 CellToWorld(Vector3Int cellPosition) => _tilemap.CellToWorld(cellPosition) + _offset;
	public Vector3 CellToLocal(Vector3Int cellPosition) => _tilemap.CellToLocal(cellPosition) + transform.localPosition + new Vector3(0.5f, 0.5f);
	public IEnumerable<Vector3Int> GetTiles<T>() where T : TileBase {
		foreach(var tile in _tilesByType[typeof(T)])
			yield return tile;
	}

	private IEnumerable<(Vector3Int position, TileBase tile)> GetTiles() {
		for(int x = _tilemap.cellBounds.xMin; x < _tilemap.cellBounds.xMax; x++) {
			for(int y = _tilemap.cellBounds.yMin; y < _tilemap.cellBounds.yMax; y++) {
				var pos = new Vector3Int(x, y, 0);
				var tile = _tilemap.GetTile(pos);
				if(tile != null)
					yield return (pos, tile);
			}
		}
	}
}
