using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnims : MonoBehaviour
{

    public List<Texture> IceList = new List<Texture>();
    public List<Texture> FireList = new List<Texture>();
    public List<Texture> PoisonList = new List<Texture>();

    public List<AudioClip> Sounds = new List<AudioClip>();


    public GameObject Parent;
    private RawImage Image;
    public AudioSource Source;
    public

    // Start is called before the first frame update
    void Start()
    {
        Image = Parent.GetComponent<RawImage>();
        
    }

    // Update is called once per frame
    private IEnumerator PlayIce()
    {

        Source.clip = Sounds[0];
        Source.Play();
        for (int i=0; i < IceList.Count; i++)
        {
            print(IceList[i].name);
            Image.texture = IceList[i];
            yield return new WaitForSeconds(0.03f);
        }



        Parent.SetActive(false);
        yield break;
    }
    private IEnumerator PlayFire()
    {

        Source.clip = Sounds[1];
        Source.Play();
        for (int i = 0; i < FireList.Count; i++)
        {
            print(FireList[i].name);
            Image.texture = FireList[i];
            yield return new WaitForSeconds(0.04f);
        }



        Parent.SetActive(false);
        yield break;
    }
    private IEnumerator PlayPoison()
    {

        Source.clip = Sounds[2];
        Source.Play();
        for (int i = 0; i < PoisonList.Count; i++)
        {
            print(PoisonList[i].name);
            Image.texture = PoisonList[i];
            yield return new WaitForSeconds(0.04f);
        }



        Parent.SetActive(false);
        yield break;
    }

    public void Decide(string M,Vector2 Pos)
    {

        Parent.GetComponent<RectTransform>().anchoredPosition = Pos;
        Parent.SetActive(true);
        if (M == "Ice")
        {
            StartCoroutine(PlayIce());
        }
        else if (M == "Fire")
        {
            StartCoroutine(PlayFire());
        }
        else if (M == "Poison")
        {
            StartCoroutine(PlayPoison());
        }
    }
}
