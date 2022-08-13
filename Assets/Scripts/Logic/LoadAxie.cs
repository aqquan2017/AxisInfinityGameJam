using System.Collections;
using System.Collections.Generic;
using AxieMixer.Unity;
using Game;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadAxie : MonoBehaviour
    {
        [SerializeField] AxieFigure _birdFigure;

        void Start()
        {
            string axieId = PlayerPrefs.GetString("selectingId", "2727");
            string genes = PlayerPrefs.GetString("selectingGenes", "0x2000000000000300008100e08308000000010010088081040001000010a043020000009008004106000100100860c40200010000084081060001001410a04406");
            _birdFigure.SetGenes(axieId, genes);
        }

        public IEnumerator GetAxiesGenes(string axieId)
        {
            string searchString = "{ axie (axieId: \"" + axieId + "\") { id, genes, newGenes}}";
            JObject jPayload = new JObject();
            jPayload.Add(new JProperty("query", searchString));

            var wr = new UnityWebRequest("https://graphql-gateway.axieinfinity.com/graphql", "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jPayload.ToString().ToCharArray());
            wr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            wr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            wr.SetRequestHeader("Content-Type", "application/json");
            wr.timeout = 10;
            yield return wr.SendWebRequest();
            if (wr.error == null)
            {
                var result = wr.downloadHandler != null ? wr.downloadHandler.text : null;
                if (!string.IsNullOrEmpty(result))
                {
                    JObject jResult = JObject.Parse(result);
                    string genesStr = (string)jResult["data"]["axie"]["newGenes"];
                    PlayerPrefs.SetString("selectingId", axieId);
                    PlayerPrefs.SetString("selectingGenes", genesStr);
                    _birdFigure.SetGenes(axieId, genesStr);
                }
            }
        }

        [ContextMenu("TEST ANIM")]
        public void TestAnimation()
        {
            List<string> animationList = Mixer.Builder.axieMixerMaterials.GetMixerStuff(AxieFormType.Normal).GetAnimatioNames();
            string animationName = animationList[Random.Range(0, animationList.Count)];
            _birdFigure.SetAnimation(animationName);
            Debug.Log(animationName);
            // foreach (var animName in animationList)
            // {
            //     Debug.Log(animName);
            // }
        }
    }
