using System;
using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
	public static DiscordController Instance;
	public static string state;
	const long APP_ID = 967743125667848212;

	public static Discord.Discord discord;
	public static ActivityManager activityManager;
	public static Discord.Activity activity;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// called first
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		string playingLevel = "Playing Level";
		switch (scene.name)
		{
			case "Main Menu":
				state = "In Main Menu";
				break;
			case "Select Level":
				state = "In Level Selection";
				break;
			case "Level 1":
				state = $"{playingLevel} 1";
				break;
			case "Level 2":
				state = $"{playingLevel} 2";
				break;
			case "Level 3":
				state = $"{playingLevel} 3";
				break;
			default:
				state = "Playing";
				break;
		}
		activity.State = state;
		UpdateActivity(activity);
	}

	void Start()
	{
		discord = new Discord.Discord(APP_ID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
		activityManager = discord.GetActivityManager();
		InitPresence();
	}

	void Update()
	{
		discord.RunCallbacks();
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnApplicationQuit() {
		discord?.Dispose();
	}

	void InitPresence()
	{
		activity.Party.Size.CurrentSize = 0;
		activity.Party.Size.MaxSize = 0;

		activity.State = SceneManager.GetActiveScene().name;
		activity.Timestamps.Start = ToUnixTime();

		activity.Assets.SmallImage = "";
		activity.Assets.SmallText = "";

		activity.Assets.LargeImage = "aee9544778991cb8928e11daea76e7b3";
		activity.Assets.LargeText = "BALLPONG X";

		UpdateActivity(activity);
	}

	void UpdateActivity(Activity _activity)
	{
		try
		{
			activityManager.UpdateActivity(_activity, (res) => {
				if (res == Discord.Result.Ok)
				{
					Debug.Log("Successfully connected to Discord client");
				}
			});
		}
		catch (Exception e)
		{
			Debug.LogWarning(e);
		}
	}

	long ToUnixTime()
	{
		DateTime date = DateTime.UtcNow;
		var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return Convert.ToInt64((date - epoch).TotalSeconds);
	}
}