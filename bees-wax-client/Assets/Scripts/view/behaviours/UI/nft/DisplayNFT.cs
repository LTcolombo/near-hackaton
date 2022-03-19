using System.Collections;
using api;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace view.behaviours.UI.nft
{
    public class DisplayNFT : MonoBehaviour
    {
        public Text title;
        public RawImage image;
        
        private NFT _data;
        public NFT Data => _data;

        public void SetData(NFT value)
        {
            _data = value;
            
            if (title != null)
                title.text = value.Title;
            
            StopAllCoroutines();
            StartCoroutine(GetTexture(value.Media));
        }

        IEnumerator GetTexture(string url)
        {
            if (url == null)
            {
                image.enabled = false;
                yield break;
            }
            
            var request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                Debug.Log(request.error);
                image.enabled = false;
            }
            else
            {
                image.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
                image.enabled = true;
            }
            
        }

        public string GetTokenId()
        {
            return _data.Id;
        }
    }
}