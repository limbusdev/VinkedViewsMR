using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Part of the player prefab. Player prefab represents the physical display
/// and it's position and rotation. With it, the client can position the pETV
/// correctly (using keyboard), so physical displays and pETV are aligned.
/// </summary>
public class ClientController : NetworkBehaviour
{
    public GameObject Anchor;
    public GameObject Frame;

    private void Start()
    {
        if(isLocalPlayer)
        {
            // Hide anchor and frame on client
            Anchor.SetActive(false);
            Frame.SetActive(false);
        }
    }
    
    void Update()
    {
        // The local player can steer pETV-object with keyboard
        if(isLocalPlayer)
        {
            // WASD define global position
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
            var y = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
            var z = Input.GetAxis("Fire1") * Time.deltaTime * 3f;
            transform.position += new Vector3(x, z, y);

            // F/V, G/B, H/N define global rotation
            var r = Input.GetAxis("Fire2") * Time.deltaTime * 50.0f;
            var s = Input.GetAxis("Fire3") * Time.deltaTime * 50.0f;
            var t = Input.GetAxis("Jump") * Time.deltaTime * 50.0f;
            transform.Rotate(r, s, t, Space.World);

            // Mouse Scrollwheel defines ETV scale
            var scale = Services.instance.clientManager.CurrentlyBoundETV.transform.localScale.x;
            scale += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 10f;
            Services.instance.clientManager.CurrentlyBoundETV.transform.localScale = new Vector3(scale, scale, scale);

            // Space triggers action on server
            if(Input.GetKeyDown(KeyCode.Space))
            {
                CmdDoOnServer();
            }
        }
    }

    // [Command] methods begin with Cmd and are called by the client, but run on the server
    [Command]
    void CmdDoOnServer()
    {
        // Disable ETV-View on Server, align it to pETV for proper 
        // VisBridges
        // Hide Anchor and Frame on Server
    }

    public override void OnStartLocalPlayer()
    {
        // Do something only for the local player prefab instance
    }

    /// <summary>
    /// Is triggered when an ETV anchor and pETV collide.
    /// This usually means, the ETVs visualization gets
    /// bound to pETV and disabled in the HoloLens App.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        // If collision is detected on Client (physical display)
        if(isLocalPlayer)
        {
            var otherAnchor = collider.gameObject.GetComponent<NetworkAnchor>();

            if(otherAnchor != null)
            {
                int dataSetID = otherAnchor.syncedDataSetID;
                string[] attributes = otherAnchor.GetAttributesAsStrings();
                var visType = (VisType)otherAnchor.syncedVisType;

                Debug.Log(attributes.Length);
                
                var vis = Services.VisFactory().GenerateVisFrom(dataSetID, attributes, visType);

                Services.instance.clientManager.CurrentlyBoundETV = vis;
            }
        }

        // If collision is detected on the server
        if(isServer)
        {
            // Disable the visualization in HoloLens and align it with pETV
        }
    }
}
