using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VektorenFormativ;

public class RocketControl : MonoBehaviour {

    public bool m_debug;        // Debugmodus

    public GameObject m_Camera;         // Kamera
    public GameObject[] m_StaticObjects; // 0 = Boden, 1 = Decke, 2 = Links, 3 = Rechts
    public float m_Thrust;    // die Kraft mit der die Rakete empor steigt
    public float m_Gravity;    // Kraft mit der die Rakete zu Boden gezogen wird
    public float m_MaxRiseSpeed;  // die maximale Geschwindigkeit mit der die Rakete nach oben fliegen kann
    public float m_FallCrashValue;    // Die Geschwindigkeit, bei der beim Landen auf den Boden ein Crash registriert werden soll
    public Material m_Red;      // Rot
    public Material m_Green;    // Gruen
    public float m_Movement; // Geschwindigkeit mit der sich in x- und z-Achse bewegt werden kann
    
    private float m_Fall; // Fallgeschwindigkeit, negativ: sinken / positiv: steigen
    private Vector m_VectorDLL;   // position of Rocket
    private bool m_CollisionRocketGround;   // wenn Rakete auf den Boden kommt -> true
    private bool m_Crash; // wenn Rakete zu schnell auf den Boden aufkommt -> true
    private float m_MovementRotation; // Geschwindigkeit mit der sich die Rakete rotiert
    private Material m_Material;    // Material des Players

    private Cuboid m_CubeRocket = new Cuboid();   // Rakete Würfel
    private Cuboid m_CubeGound = new Cuboid();    // Boden Würfel
    private Cuboid[] m_StaticCubes;                 // Objecte als Würfel
    private bool[] m_CalcCollisionCube;                 // wenn auf true --> Collision muss berechnet werden
    private bool[] m_col;                           // bei dem Objekt, bei dem es kollidiert --> true. daraus wird "m_CollisionRocketGround" auf true gesetzt

    private Transform[] m_TransMet; // Transform von den Meteoriten
    private Sphere[] m_Meteorite;   // Meteoriten in SphereDLL
    private bool[] m_CalcCollisionSphere;   // wenn auf true --> Collision muss berechnet werden

    // Use this for initialization
    void Start ()
    {
        // ClacCollosion Array erstellen
        m_CalcCollisionCube = new bool[m_StaticObjects.GetLength(0)];

        // m_col array erstellen
        m_col = new bool[m_StaticObjects.GetLength(0)];


        // Cuboid Array erstellen
        m_StaticCubes = new Cuboid[m_StaticObjects.GetLength(0)];

        // Material Rot für wenn Kollidiert
        m_Material = GetComponent<Renderer>().material;

        // Set Rotation
        m_MovementRotation = Random.Range(-10f, 10f);

        // set Transform of Spheres
        m_TransMet = GetMeteorites.m_ChildrenObstacles;

        Debug.Log(GetMeteorites.m_ChildrenObstacles.GetLength(0));
        m_Meteorite = new Sphere[GetMeteorites.m_ChildrenObstacles.GetLength(0)];

        m_CalcCollisionSphere = new bool[m_Meteorite.GetLength(0)];

        // Eckpunkte des Bodens
        for (int i = 0; i < m_StaticObjects.GetLength(0); i++)
        {
            m_StaticCubes[i] = new Cuboid(CubeEckPosition.GetCubeVectorPosition(m_StaticObjects[i]));
        }
            GetMeteoritesFromCS();
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_debug)
            CheckCollisionMeteoritePlayer();

        if (m_Meteorite == null)
        {
            Debug.Log("m_Meteorite = Null");
            GetMeteoritesFromCS();
            return;
        }
        // Wenn Fallgeschwindigkeit zu Hoch war beim Auftritt auf den Boden
        if (m_Crash)
            return;

        // Abfrage ob Taste gedrückt wurde
        if (Input.GetKey(KeyCode.Space))
        {
            // wenn Space gedrückt wurde
            // rise
            m_Fall += m_Thrust * m_Thrust;
        }
        else
        {
            // wenn Space nicht gedrückt wurde
            // fall
            // überprüfen ob Rakete auf dem Boden ist
            if (!m_CollisionRocketGround && !m_Crash)
                m_Fall -= m_Gravity * m_Gravity;
            else
                m_Fall = 0;
        }

        m_CollisionRocketGround = false;


        // Get position of Rocket
        m_VectorDLL = VectorUmwandeln(transform.position);

        // Rakete nach links und rechts bewegen
        if (Input.GetKey(KeyCode.A))
        {
            // X -
            m_VectorDLL.X -= m_Movement;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // X +
            m_VectorDLL.X += m_Movement;
        }

        // Rakete rotieren
        if(Input.GetKey(KeyCode.Q))
        {
            // Y -
            m_MovementRotation += m_Movement / 90;
        }
        if (Input.GetKey(KeyCode.E))
        {
            // Y +
            m_MovementRotation += -(m_Movement / 90);
        }


        // cap rise speed
        if (m_Fall >= m_MaxRiseSpeed)
            m_Fall = m_MaxRiseSpeed;

        // Berechne die Fallgeschwindigkeit der Y-Achse hinzu
        m_VectorDLL.Y += m_Fall;


        // Eckpunkte der Rakete herausfinden + setzen
        m_CubeRocket = new Cuboid(CubeEckPosition.GetCubeVectorPosition(this.gameObject));


        // set bool whether to calculate collision or not
        for (int i = 0; i < m_CalcCollisionCube.GetLength(0); i++)
        {
            Vector tempRocket = VectorUmwandeln(this.transform.position); // Um die Entfernung des Objekts und des Plaxers zu berechnen
            Vector tempStaticObjects = 
                tempRocket - 
                new Vector(
                    m_StaticObjects[i].transform.position.x,
                    m_StaticObjects[i].transform.position.y,
                    m_StaticObjects[i].transform.position.z)
                    ;

            if ((tempRocket.X >= tempStaticObjects.X - 250f && tempRocket.X <= tempStaticObjects.X + 250) ||
                (tempRocket.Y >= tempStaticObjects.Y - 500f && tempRocket.Y <= tempStaticObjects.Y + 500))
            {
                m_CalcCollisionCube[i] = true;
            }
            else
                m_CalcCollisionCube[i] = false;
        }
        // check collision of ground, top, left and right block
        for (int i = 0; i < m_StaticObjects.GetLength(0); i++)
        {
            if (m_CalcCollisionCube[i] == false)
                continue;

            m_col[i] = Collisions.CuboidInCuboid(m_CubeRocket, m_StaticCubes[i]);

        }
        // Collision Boden setzen
        if (m_col[0])
            m_CollisionRocketGround = true;
        // Collision Wand und Decke setzen
        for (int i = 1; i < m_col.GetLength(0); i++)
        {
            if (m_col[i])
                m_Crash = true;
        }

        // check collision of Meteorites
        CheckCollisionMeteoritePlayer();
        if (!m_Crash)
        {


            // wenn nichts kollidiert -> CollisionRocketGround = false
            bool collidedOnce = false;
            for (int i = 0; i < m_col.GetLength(0); i++)
            {
                if (m_col[i])
                {
                    collidedOnce = true;
                }
            }
            if (collidedOnce == false)
                m_CollisionRocketGround = false;

            // überprüft ob die Rakete halbwegs gerade aufgekommen ist (+-10 Grad)
            if (m_CollisionRocketGround)
            {
                float checkRot = this.gameObject.transform.rotation.eulerAngles.z;
                checkRot %= 90f;
                if (checkRot < 80f && checkRot > 10f)
                    m_Crash = true;

                // fall check
                if (!m_debug)
                    if (m_Fall <= m_FallCrashValue)
                    {
                        Debug.Log(m_Fall);
                        m_Crash = true;
                    }
            }
        }

        // Position setzen
        if (!m_CollisionRocketGround)
        {
            // setze neue Position
            if(!m_debug)
                transform.position = VectorUmwandeln(m_VectorDLL);

            // setzt neue Rotation
            transform.localRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + m_MovementRotation);

            // setze Rotation von Kamera zurück
            m_Camera.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        // Wenn Rakete sanft landet
        else if (m_CollisionRocketGround && !m_Crash)
        {
            m_Material.CopyPropertiesFromMaterial(m_Green);
        }

        if (m_Crash)
        {
            m_Material.CopyPropertiesFromMaterial(m_Red);
            //m_Material = m_Red;
        }

    }
    // ----------------------------------------------------------- //

    /// <summary>
    /// Wandelt DLL Vector in Unity Vector um
    /// </summary>
    /// <param name="_v">DLL Vector</param>
    /// <returns>Unity Vector</returns>
    private Vector3 VectorUmwandeln(Vector _v)
    {
        Vector3 v = new Vector3(_v.X, _v.Y, _v.Z);
        return v;
    }

    /// <summary>
    /// Wandelt Unity Vector in DLL Vector um
    /// </summary>
    /// <param name="_v">Unity Vector</param>
    /// <returns>DLL Vector</returns>
    private Vector VectorUmwandeln(Vector3 _v)
    {
        Vector v = new Vector(_v.x, _v.y, _v.z);
        return v;
    }

    /// <summary>
    /// Set Meteorite Data
    /// </summary>
    private void GetMeteoritesFromCS()
    {
        // if Meteorites not initiated return
        Debug.Log(GetMeteorites.m_ChildrenObstacles[0].position);
        if (GetMeteorites.m_ChildrenObstacles == null)
        {
            return;
        }

        for (int i = 0; i < m_Meteorite.GetLength(0); i++)
        {
            m_Meteorite[i] = new Sphere(new Vector(0,0,0),0);
        }

        // get meteorites array
        for (int i = 0; i < m_TransMet.GetLength(0); i++)
        {

            //Debug.Log("Schleife " + i);
            //Debug.Log("Transmet " + i + "Position = " + VectorUmwandeln( m_TransMet[i].position));
            m_Meteorite[i].m_Center = new Vector(1f,2f,3f);

            

            // set Center
            m_Meteorite[i].m_Center = VectorUmwandeln(m_TransMet[i].position);
            // set Radius
            m_Meteorite[i].m_Radius = m_TransMet[i].lossyScale.x / 2;
        }
    }

    private void CheckCollisionMeteoritePlayer()
    {
        // check whether collision must be checked
        Vector tempRocket = VectorUmwandeln(this.gameObject.transform.position);
        for (int i = 0; i < m_Meteorite.GetLength(0); i++)
        {
            // berechne Distanz
            float distance = Vector.SqrMagnitude(tempRocket - VectorUmwandeln(m_TransMet[i].position));
            if (distance <= ((m_Meteorite[i].m_Radius*2) * (m_Meteorite[i].m_Radius*2)))
                m_CalcCollisionSphere[i] = true;
            else
                m_CalcCollisionSphere[i] = false;
        }

        // check collision
        for (int i = 0; i < m_CalcCollisionSphere.GetLength(0); i++)
        {
            if (!m_CalcCollisionSphere[i])
                continue;

            if (m_debug)
                Debug.Log(i + " " + Collisions.CuboidInSphere(m_CubeRocket, m_Meteorite[i]));
            else
            {
                m_Crash = Collisions.CuboidInSphere(m_CubeRocket, m_Meteorite[i]);
                if (m_Crash)
                    return;
            }
        }
    }

}
