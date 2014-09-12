using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor( typeof(AIController))]
public class AIController_Editor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // show original inspector
        var aiController = target as AIController;
        GUILayoutOption[] options = new GUILayoutOption[0];

        if (aiController.behaviors.Contains(AIController.Behavior.FREEROAM) ||
            aiController.behaviors.Contains(AIController.Behavior.FLYSTRAIGHT))
        {
            aiController.acceleration = EditorGUILayout.FloatField("Acceleration:", aiController.acceleration);
            aiController.distance = EditorGUILayout.FloatField("Distance: ", aiController.distance); // this is somethin we want to show, not set..

        }
        if (aiController.behaviors.Contains(AIController.Behavior.ROTATE))
        {
            aiController.rotationSpeed = EditorGUILayout.FloatField("RotationSpeed: ", aiController.rotationSpeed);
            /*
            EditorGUILayout.BeginHorizontal("RotationAxis:");
            float x = 0;
            float y = 0;
            float z = 0;
            EditorGUILayout.LabelField("RotationAxis");
            x = EditorGUILayout.FloatField(x);
            y = EditorGUILayout.FloatField(y);
            z = EditorGUILayout.FloatField(z);
            aiController.rotationAxis = new Vector3(x, y, z);
            EditorGUILayout.EndHorizontal();
             * */
            
        }
        if (aiController.behaviors.Contains(AIController.Behavior.SHOOTATPLAYER))
        {
            aiController.ShotPointNetwork = EditorGUILayout.ObjectField("ShotPointNetwork:", aiController.ShotPointNetwork, typeof(GameObject)) as GameObject;
            aiController.attackDistance = EditorGUILayout.IntField("AttackDistance: ", aiController.attackDistance);
            aiController.fireDistance = EditorGUILayout.IntField("FireDistance: ", aiController.fireDistance);
            aiController.fireRate = EditorGUILayout.FloatField("FireRate(1/x): ", aiController.fireRate);
            aiController.Projectile = EditorGUILayout.ObjectField("Projectile:", aiController.Projectile, typeof(GameObject)) as GameObject;
            
        }
        if(aiController.behaviors.Contains(AIController.Behavior.SPAWNENEMIES))
        {
            //aiController.EnemiesToSpawn = EditorGUILayout.
            aiController.SpawnPointNetwork = EditorGUILayout.ObjectField("SpawnPointNetwork:", aiController.SpawnPointNetwork, typeof(GameObject)) as GameObject;
            aiController.spawnRate = EditorGUILayout.FloatField("SpawnRate: ",aiController.spawnRate);
            aiController.maxEnemySpawns = EditorGUILayout.IntField("MaxEnemySpawns", aiController.maxEnemySpawns);

        }
        if (aiController.behaviors.Contains(AIController.Behavior.UNLOAD))
        {
            aiController.unloadTime = EditorGUILayout.FloatField("UnloadTime:", aiController.unloadTime);
        }

        
    }

}
