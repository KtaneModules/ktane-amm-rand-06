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

	void Awake() { videoPlayer.SetActive(false); }
	public IEnumerator playRandomVideo()
	{
		int ind = Random.Range(0, 64);
		if (!Application.isEditor && externalSolveVideos == null)
			externalSolveVideos = PathManager.GetAssets<VideoClip>("ammvideo");
		Video.clip = (!Application.isEditor ? externalSolveVideos : solveVideos)[ind];
		Video.Play();
		videoPlayer.SetActive(true);
		Audio.PlaySoundAtTransform(solveAudios[ind].name, transform);
		yield return new WaitForSeconds(solveAudios[ind].length);
		videoPlayer.SetActive(false);
	}
}
