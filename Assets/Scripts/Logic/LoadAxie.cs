using System.Collections;
using System.Collections.Generic;
using AxieCore.AxieMixer;
using AxieMixer.Unity;
using Game;
using Newtonsoft.Json.Linq;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEditor.ObjectChangeEventStream;

public class LoadAxie : MonoBehaviour
    {
        [SerializeField] AxieFigure _birdFigure;
        [SerializeField] int classIdx = 0;
        [SerializeField] int classValue = 2;
        [SerializeField] bool flipX = true;
    [SerializeField] bool isPlayer = true;

    Axie2dBuilder builder => Mixer.Builder;

    void Start()
        {
            //string axieId = PlayerPrefs.GetString("selectingId", "2727");
            //string genes = PlayerPrefs.GetString("selectingGenes", "0x2000000000000300008100e08308000000010010088081040001000010a043020000009008004106000100100860c40200010000084081060001001410a04406");
            //_birdFigure.SetGenes(axieId, genes);

        TestAll();
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
            _birdFigure.SetAnimation(animationName, 1, true);
            Debug.Log(animationName);
            // foreach (var animName in animationList)
            // {
            //     Debug.Log(animName);
            // }
        }

    void TestAll()
    {
        List<(string, string, int, int)> bodies = new List<(string, string, int, int)>();
        string[] specialBodys = new[]
        {
                "body-normal",
                "body-bigyak",
                "body-curly",
                "body-fuzzy",
                "body-spiky",
                "body-sumo",
                "body-wetdog",
            };

        int k = 0;
        var characterClass1 = (CharacterClass)classIdx;
        string key1 = $"{characterClass1}-{classValue:00}";
        //
        bodies.Add((key1, specialBodys[(k++) % specialBodys.Length], classIdx, classValue));

        //for (int classIdx = 0; classIdx < 6; classIdx++)
        //{
        //    var characterClass = (CharacterClass)classIdx;
        //    string key = $"{characterClass}-mystic-02";
        //    bodies.Add((key, (classIdx % 2 == 0) ? "body-mystic-normal" : "body-mystic-fuzzy", classIdx, 2));
        //}

        //{
        //    for (int classValue = 1; classValue <= 2; classValue += 1)
        //    {
        //        string key = $"xmas-{classValue:00}";
        //        bodies.Add((key, "body-frosty", 0, classValue));
        //    }
        //}
        //{
        //    for (int classValue = 1; classValue <= 3; classValue += 1)
        //    {
        //        string key = $"japan-{classValue:00}";
        //        bodies.Add((key, "body-normal", 0, classValue));
        //    }
        //}
        //{
        //    for (int classValue = 0; classValue <= 1; classValue += 1)
        //    {
        //        string key = $"agamo-{classValue:00}";
        //        bodies.Add((key, "body-agamo", 0, classValue));
        //    }
        //}

        int total = 0;
        foreach (var (key, body, classIdx, classValue) in bodies)
        {
            var characterClass = (CharacterClass)classIdx;
            string finalBody = body;
            string keyAdjust = key.Replace("-06", "-02").Replace("-12", "-04");
            var adultCombo = new Dictionary<string, string> {
                    {"back", key },
                    {"body", finalBody },
                    {"ears", key },
                    {"ear", key },
                    {"eyes", keyAdjust },
                    {"horn", key },
                    {"mouth", keyAdjust },
                    {"tail", key },
                    {"body-class", characterClass.ToString() },
                {"body-id", " 2727 " },
            };

            float scale = 0.0018f;
            byte colorVariant = (byte)builder.GetSampleColorVariant(characterClass, classValue);

            var builderResult = builder.BuildSpineAdultCombo(adultCombo, colorVariant, scale);

            SkeletonAnimation runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(builderResult.skeletonDataAsset);
            //runtimeSkeletonAnimation.gameObject.layer = LayerMask.NameToLayer("Player");
            runtimeSkeletonAnimation.transform.SetParent(this.transform, false);
            runtimeSkeletonAnimation.transform.localPosition = new Vector3(0, -.5f, 0);
            runtimeSkeletonAnimation.transform.localScale = Vector3.one;
            var meshRenderer = runtimeSkeletonAnimation.GetComponent<MeshRenderer>();
            meshRenderer.sortingOrder = 10 * total;
            total++;

            runtimeSkeletonAnimation.gameObject.AddComponent<AutoBlendAnimController>();
            runtimeSkeletonAnimation.state.SetAnimation(0, "action/idle/normal", true);

            runtimeSkeletonAnimation.state.TimeScale = 0.5f;
            runtimeSkeletonAnimation.skeleton.FindSlot("shadow").Attachment = null;
            if (builderResult.adultCombo.ContainsKey("body") &&
                  builderResult.adultCombo["body"].Contains("mystic") &&
                  builderResult.adultCombo.TryGetValue("body-class", out var bodyClass) &&
                  builderResult.adultCombo.TryGetValue("body-id", out var bodyId))
            {
                runtimeSkeletonAnimation.gameObject.AddComponent<MysticIdController>().Init(bodyClass, bodyId);
            }

            runtimeSkeletonAnimation.gameObject.AddComponent<AxieFigure>();
            if (isPlayer)
            {
                this.gameObject.GetComponent<PlayerMovement>()._axieFigure = runtimeSkeletonAnimation.GetComponent<AxieFigure>();
            }
            runtimeSkeletonAnimation.GetComponent<AxieFigure>().FlipX = flipX;
        }
        Debug.Log("Done");
    }

}
