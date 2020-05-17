using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public City player;
    public Camera camera;
    public ActionCard hovered;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            hovered = hit.transform.gameObject.GetComponent<ActionCard>();

            if (hovered != null) {
                if (Input.GetMouseButtonDown(0))
                {
                    if (player.CanPlay(hovered)){
                        Debug.Log("you can play this card");
                        player.Play(hovered);
                    }
                    else
                    {
                        Debug.Log("you CANNOT play this card");
                    }
                }
            }
        }
        else
        {
            hovered = null;
        }
    }
}
