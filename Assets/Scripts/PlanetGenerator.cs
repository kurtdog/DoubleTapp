using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Planetary;

public class PlanetGenerator : MonoBehaviour {

	public GameObject Ship;
    public GameObject Map;
    public GameObject AssetBuilder;
    public GameObject EmptyTextPrefab;//used for the guiPopupPlanets, they have text objects
	//public GameObject camera;
	public List<TextAsset> PlanetModules;
	public List<string> TerrainTextureNames;
	public List<string> AtmosphereTextureNames;
	public List<string> HydrosphereTextureNames;
    public List<EnemySet> EnemySets;
	public GameObject PlanetPrefab;
	public Material PlanetMaterialPrefab;
	//public Material AtmosphereMaterialPrefab;
	//public Material HydrosphereMaterialPrefab;
	public GameObject HydrospherePrefab;
	public GameObject AtmospherePrefab;
    public GameObject AsteroidBeltPrefab;

	public int minRadius;
	public int maxRadius;
	//public double minHeightVariation;
	//public double maxHeightVariation;
	public int meshResolution;
	public int subdivisions;
	public bool generateOnStart;
	public float randomSeed;

	public bool generateAtmosphere;
	public bool generateHydrosphere;
    public bool generateAsteroidBelt;

	public GameObject generatedPlanet;
	public GameObject generatedGuiPopupPlanet;
	public GameObject generatedAtmosphere;
	public GameObject generatedHydrosphere;
    public GameObject generatedAsteroidBelt;

	//TODO: Generate planet button? 

	// Use this for initialization
	void Start () {

       TerrainTextureNames = LoadFileNames("Assets/Resources/TerrainTextures");
       AtmosphereTextureNames = LoadFileNames("Assets/Resources/AtmosphereTextures");
       HydrosphereTextureNames = LoadFileNames("Assets/Resources/HydrosphereTextures");

        if (generateOnStart)
        {
            GeneratePlanet(this.transform.position);
        }
  
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject GeneratePlanet(Vector3 position)
	{
		generatedPlanet = Instantiate (PlanetPrefab, position, this.transform.rotation) as GameObject;
		Planet planetScript = generatedPlanet.GetComponent<Planet> ();
		//Generate Tab
		planetScript.terrainAsset = GetRandomModule();
		planetScript.radius = Random.Range (minRadius, maxRadius);
		//we want a height var, wiht a value at least in the 10's
		float heightVar = Random.value;
		while (heightVar > .1) {
			heightVar *= Random.value;
		}
		planetScript.heightVariation = heightVar;
		planetScript.meshResolution = meshResolution;
		planetScript.subdivisions = subdivisions;
		planetScript.generateOnStart = generateOnStart;
		planetScript.seed = randomSeed;
		planetScript.generateColliders [0] = true;
		//LOD tab
		planetScript.lodTarget = Ship.transform;
		//planetScript.lodUpdateInterval = .25f;
		planetScript.maxLodLevel = 5;
		float radius = planetScript.radius;
		planetScript.lodDistances [0] = radius*2;
		planetScript.lodDistances [1] = radius + radius*.5f;
		planetScript.lodDistances [2] = radius;
		planetScript.lodDistances [3] = radius*.5f;
		planetScript.lodDistances [4] = radius*.25f;
		planetScript.useLod = true;
		//Shader tab
		Material randomMaterial = new Material (PlanetMaterialPrefab); // create a random material, of the Plannertary/Terrain type
        randomMaterial.SetTexture("_SlopeTexture", GetRandomTexture(TerrainTextureNames, "TerrainTextures/")); // assign each texture in the material
        randomMaterial.SetTexture("_Texture1", GetRandomTexture(TerrainTextureNames, "TerrainTextures/"));
        randomMaterial.SetTexture("_Texture2", GetRandomTexture(TerrainTextureNames, "TerrainTextures/"));
        randomMaterial.SetTexture("_Texture3", GetRandomTexture(TerrainTextureNames, "TerrainTextures/"));
        randomMaterial.SetTexture("_Texture4", GetRandomTexture(TerrainTextureNames, "TerrainTextures/"));
		planetScript.terrainMaterial = randomMaterial; //set the material for our planet.

		planetScript.Generate (null);

        generatedPlanet.AddComponent<MapObject>();
		GenerateGuiPopupPlanet ();
        Map.GetComponent<Map>().mapObjects.Add(generatedPlanet); // add this planet to the ship's map


        //generate a hydrosphere and atmostphere if the user selected to do so, or by random chance
        float seed1 = Random.value;
        float seed2 = Random.value;
        float seed3 = Random.value;
        //generate atmosphere and hydrosphere
        if (generateAtmosphere || seed1 < .5f)
        {
            GenerateAtmosphere();
        }
        if (generateHydrosphere || seed2 < .5f)
        {
            GenerateHydrosphere();
        }
        if (generateAsteroidBelt || seed3 < .5f)
        {
            GenerateAsteroidBelt();
        }


        //Setup the enemies to be generated for this Planet. Using the PlanetEnemyGenerator script.
        PlanetEnemyGenerator enemyGenerator = generatedPlanet.AddComponent<PlanetEnemyGenerator>();
        enemyGenerator.enemySet = GetRandomEnemySet();
        enemyGenerator.planet = generatedPlanet;
        enemyGenerator.Ship = Ship;
        enemyGenerator.assetBuilder = AssetBuilder.GetComponent<AssetBuilder>();

        return generatedPlanet;
	}

	void GenerateGuiPopupPlanet()
	{
        generatedGuiPopupPlanet = Instantiate(PlanetPrefab, generatedPlanet.transform.position, this.transform.rotation) as GameObject; // instantiate a new empty planet
        generatedGuiPopupPlanet.AddComponent<GuiPopup>(); // this generated planet is a guiPopup, add the guiPopupScript
		Planet guiPopupPlanetScript = generatedGuiPopupPlanet.GetComponent<Planet> (); // get the planet script of our new guiPopupPlanet
		Planet planetScript = generatedPlanet.GetComponent<Planet>(); // get the script from the big generated planet
		//copy over some values from the big planet to the new small one
		guiPopupPlanetScript.terrainAsset = planetScript.terrainAsset;
		guiPopupPlanetScript.heightVariation = planetScript.heightVariation;
		guiPopupPlanetScript.meshResolution = planetScript.meshResolution;
		guiPopupPlanetScript.subdivisions = planetScript.subdivisions;
		guiPopupPlanetScript.terrainMaterial = planetScript.terrainMaterial; //set the material for our planet.
		guiPopupPlanetScript.radius = planetScript.radius/(maxRadius*2); // radius of small planet is small
		guiPopupPlanetScript.Generate (null); //generate the tiny planet

		generatedGuiPopupPlanet.layer = LayerMask.NameToLayer("NoLight"); //set the layer
		generatedGuiPopupPlanet.GetComponent<MeshRenderer> ().enabled = false; // disable the mesh renderer

        //set the script variables related to the map, MapObject and GuiPopup
		generatedPlanet.GetComponent<MapObject>().guiPopupPrefab = generatedGuiPopupPlanet;
        generatedGuiPopupPlanet.GetComponent<GuiPopup>().mapObject = generatedPlanet;

        //add a FloatingText script and MapObjectTextController script, and initialize the correct variables
        FloatingText floatingText = generatedGuiPopupPlanet.AddComponent<FloatingText>();
        floatingText.ParentObject = generatedGuiPopupPlanet.gameObject;
        floatingText.EmptyTextPrefab = this.EmptyTextPrefab;
        floatingText.anchor = TextAnchor.MiddleCenter;
        floatingText.alignment = TextAlignment.Center;
        floatingText.spacing = 1;
        MapObjectTextController textController = generatedGuiPopupPlanet.AddComponent<MapObjectTextController>();
        textController.Ship = this.Ship;
        textController.MapObject = generatedPlanet;
        
	}

	public void GenerateAtmosphere()
	{
        generatedAtmosphere = Instantiate(AtmospherePrefab, generatedPlanet.transform.position, this.transform.rotation) as GameObject; // create an atmosphere object
		// set the radius
		//float radius =  Random.Range (minRadius, maxRadius); // random
		float planetRadius = generatedPlanet.GetComponent<Planet>().radius/10;// from planet
		float radius = planetRadius + planetRadius * Random.value; // up to 2 x the radius of the planet
		//float radius = 1 + Random.value*2;
		generatedAtmosphere.transform.parent = generatedPlanet.transform;
		generatedAtmosphere.transform.localScale = new Vector3(radius,radius,radius);
		generatedAtmosphere.layer = LayerMask.NameToLayer ("NoLight");
		//generatedAtmosphere.GetComponent<Shader>().SetTexture ("_MainTex",GetRandomTexture("AtmosphereTextures/"));
		generatedAtmosphere.renderer.material.SetTexture("_MainTex",GetRandomTexture(AtmosphereTextureNames,"AtmosphereTextures/"));

	}

    void GenerateGuiPopupAtmosphere()
    {
        GameObject generatedGuiPopupAtmosphere = Instantiate(AtmospherePrefab, generatedPlanet.transform.position, this.transform.rotation) as GameObject; // instantiate a new empty planet

        generatedGuiPopupAtmosphere.layer = LayerMask.GetMask("NoLight");
        generatedGuiPopupAtmosphere.GetComponent<MeshRenderer>().enabled = false;
        float radius = generatedAtmosphere.transform.lossyScale.magnitude / (maxRadius * 2);
        generatedAtmosphere.transform.localScale = new Vector3(radius, radius, radius);
        generatedAtmosphere.renderer.material.SetTexture("_MainTex", generatedAtmosphere.renderer.material.GetTexture(0));

        generatedGuiPopupAtmosphere.transform.parent = generatedGuiPopupPlanet.transform;

    }

	public void GenerateHydrosphere()
	{
        generatedHydrosphere = Instantiate(HydrospherePrefab, generatedPlanet.transform.position, this.transform.rotation) as GameObject; // create an atmosphere object
		// set the radius
		//float radius =  Random.Range (minRadius, maxRadius); // random
		float planetRadius = generatedPlanet.GetComponent<Planet>().radius;// from planet
		float radius = planetRadius;// + planetRadius * Random.value; // up to 2 x the radius of the planet
		//float radius = 1 + Random.value*2;
		generatedHydrosphere.transform.parent = generatedPlanet.transform;
		generatedHydrosphere.transform.localScale = new Vector3(radius,radius,radius);
		generatedHydrosphere.layer = LayerMask.NameToLayer ("NoLight");
		generatedHydrosphere.renderer.material.SetTexture("_MainTex",GetRandomTexture(HydrosphereTextureNames,"HydrosphereTextures/"));
		generatedHydrosphere.GetComponent<WaterShader> ().mainCamera = Camera.main;
	}

    void GenerateGuiPopupHydrosphere()
    {
        GameObject generatedGuiPopupHydrosphere = Instantiate(HydrospherePrefab, generatedPlanet.transform.position, this.transform.rotation) as GameObject; // instantiate a new empty planet


        generatedGuiPopupHydrosphere.GetComponent<MeshRenderer>().enabled = false;
        float radius = generatedGuiPopupPlanet.transform.lossyScale.magnitude;
        generatedGuiPopupHydrosphere.transform.localScale = new Vector3(radius, radius, radius);
        generatedGuiPopupHydrosphere.layer = LayerMask.NameToLayer("NoLight");
        generatedGuiPopupHydrosphere.renderer.material.SetTexture("_MainTex", generatedAtmosphere.renderer.material.GetTexture(0));

        generatedGuiPopupHydrosphere.transform.parent = generatedGuiPopupPlanet.transform;

    }

    void GenerateAsteroidBelt()
    {

        generatedAsteroidBelt = Instantiate(AsteroidBeltPrefab, generatedPlanet.transform.position, Random.rotation) as GameObject;
        AsteroidBeltSc asteroidBelt = generatedAsteroidBelt.GetComponent<AsteroidBeltSc>();
        //float atmosphereRadius = generatedAtmosphere.transform.lossyScale.magnitude; // get the atmosphere's radius
        float radius = generatedPlanet.GetComponent<Planet>().radius;
        asteroidBelt.radiusInnerRing = (int)(radius + radius * Random.value);// we want it to be at least as big as the atmosphere + some
        asteroidBelt.radiusOuterRing = (int)(asteroidBelt.radiusInnerRing + asteroidBelt.radiusInnerRing * .2f); // the outer ring should be > inner ring
        //TODO: assign the asteroid's texture = the planet texture
        generatedAsteroidBelt.GetComponent<Generator>().assetBuilder = AssetBuilder.GetComponent<AssetBuilder>();

    }

	TextAsset GetRandomModule()
	{
		int i = Random.Range (0, PlanetModules.Count - 1);
		return PlanetModules [i];
	}

	Texture GetRandomTexture(List<string> textureList, string path)
	{

        Texture t = new Texture();
        while (t == null)
        {
            int index = Random.Range(0, textureList.Count - 1);
            Debug.Log(textureList.ToString() + " : " + textureList.Count);
            string s = textureList[index];
            Debug.Log("Getting Texture: " + s);
            t = Resources.Load(path + s) as Texture;
        }
		return t;//return null;

	}

    List<string> LoadFileNames(string path)
    {
        List<string> fileList = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            string tempName = f.Name;
            string extension = f.Extension;
            string strippedName = tempName.Replace(extension, "");
            fileList.Add(strippedName);
        }

        return fileList;
    }

    public EnemySet GetRandomEnemySet()
    {
        int index = Random.Range(0,EnemySets.Count);
        return EnemySets[index];
    }

}
