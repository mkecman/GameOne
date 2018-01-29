using UnityEngine;
using System.Collections;
using AccidentalNoise;

public class HexMap : MonoBehaviour
{
    public GameObject HexagonPrefab;
    public int width = 64;
    public int height = 40;

    private float xOffset = 0.882f;
    private float zOffset = 0.764f;

    // Use this for initialization
    void Start()
    {
        
        int TerrainOctaves = 6;
        double TerrainFrequency = 1.25;
        // Initialize the HeightMap Generator
        ImplicitFractal HeightMap = new ImplicitFractal( FractalType.MULTI,
                                           BasisType.SIMPLEX,
                                           InterpolationType.QUINTIC,
                                           TerrainOctaves,
                                           TerrainFrequency,
                                           UnityEngine.Random.Range( 0, int.MaxValue ) );

        double min = 0;
        double max = 0;

        for( int x = 0; x < width; x++ )
        {
            for( int y = 0; y < height; y++ )
            {
                float xPos = x * xOffset;
                // Are we on an odd row?
                if( y % 2 == 1 )
                {
                    xPos += xOffset / 2f;
                }

                if( HeightMap.Get( x, y ) < min )
                    min = HeightMap.Get( x, y );
                if( HeightMap.Get( x, y ) > max )
                    max = HeightMap.Get( x, y );

                GameObject hex_go = (GameObject)Instantiate( HexagonPrefab, new Vector3( xPos, (float)HeightMap.Get( x, y ), y * zOffset), Quaternion.identity );
                hex_go.name = "Hex_" + x + "_" + y;
                // Make sure the hex is aware of its place on the map
                hex_go.GetComponent<Hex>().X = x;
                hex_go.GetComponent<Hex>().Y = y;

                // For a cleaner hierachy, parent this hex to the map
                hex_go.transform.SetParent( this.transform );

                // TODO: Quill needs to explain different optimization later...
                hex_go.isStatic = true;

            }
        }

        Debug.Log( "min: " + min + ":::::" + "max: " + max );

    }
}
