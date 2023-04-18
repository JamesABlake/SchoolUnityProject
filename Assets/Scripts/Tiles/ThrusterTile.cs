using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[MyTile]
public class ThrusterTile : RuleTile {
#if UNITY_EDITOR
	// The following is a helper that adds a menu item to create a MyTile Asset
	[MenuItem("Assets/Create/MyTiles")]
	public static void CreateMyTile() {
		Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(type => type.IsDefined(typeof(MyTileAttribute)))).ToArray();

		string path = EditorUtility.OpenFolderPanel("Save My Tiles", "Assets", "");
		path = "Assets" + path[Application.dataPath.Length..];
		if(path == "")
			return;
		foreach(Type type in types) {
			AssetDatabase.CreateAsset(CreateInstance(type), $"{path}/{type}.asset");
		}

	}
#endif
}
