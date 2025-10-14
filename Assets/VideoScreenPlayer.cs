using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using KeepCoding;

public class VideoScreenPlayer : MonoBehaviour {

	public VideoPlayer Video;
	public VideoClip[] solveVideos;
	private static VideoClip[] externalSolveVideos;
	public KMAudio Audio;
	public AudioClip[] solveAudios;
	public GameObject videoPlayer;

	public VideoClip staticVideo;
	public AudioClip staticAudio;
	public SpriteRenderer spriteRenderer;
	public Sprite[] images;

	private KMAudio.KMAudioRef _audio;
	private bool lockStatic;

	void Awake()
	{
		videoPlayer.SetActive(false); 
		//spriteRenderer.GetComponent<GameObject>().SetActive(false);
		spriteRenderer.enabled = false;
		if (!Application.isEditor && externalSolveVideos == null)
		{
			externalSolveVideos = PathManager.GetAssets<VideoClip>("ammvideo");
			staticVideo = externalSolveVideos.Find(i => i.name == "output.webm");
		}
		Debug.Log("init");
		Video.clip = staticVideo;
		if (Video.clip == null) Debug.LogErrorFormat("Video clip is null");
		else Debug.LogFormat("Video clip is {0}", Video.clip.name);
		videoPlayer.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .5f);
	}
	public IEnumerator playRandomVideo()
	{
		lockStatic = true;
		int ind = Random.Range(0, 64);
		Video.clip = (!Application.isEditor ? externalSolveVideos : solveVideos)[ind];
		Video.Play();
		videoPlayer.SetActive(true);
		Audio.PlaySoundAtTransform(solveAudios[ind].name, transform);
		yield return new WaitForSeconds(solveAudios[ind].length);
		videoPlayer.SetActive(false);
	}

	public IEnumerator spamPictures(int config = -1)
	{
		
		
		
		yield return null;
	}
	
	public IEnumerator playStatic(float duration)
	{
		if (lockStatic) yield break;
		Video.Play();
		videoPlayer.SetActive(true);
		_audio = Audio.PlaySoundAtTransformWithRef(staticAudio.name, transform);
		yield return new WaitForSeconds(duration);
		_audio.StopSound();
		Video.Stop();
		videoPlayer.SetActive(false);
	}
	
}
