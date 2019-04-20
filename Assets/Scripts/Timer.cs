using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
	private float countTo = 0;
	private bool done = true;
	private float cur = 0;
	private bool paused = false;

	public Timer(float seconds) {
		this.countTo = seconds;
	}

	public bool Started = false;
	/**
	 * Starts the timer
	 */
	public void Start() {
		this.Started = true;
		this.done = false;
		this.cur = Time.realtimeSinceStartup;
	}

	public void Stop() {
		this.Started = false;
		this.done = false;
	}


	bool wasPaused;

	public void Update() {
		if (!this.Started) {
			this.Start();
		}

		if (this.paused) {
			wasPaused = true;
			return;
		} else if (this.wasPaused) {
			wasPaused = false;
			this.cur = Time.realtimeSinceStartup - GetProgressTime();
		}

		if (this.cur + this.countTo < Time.realtimeSinceStartup) {
			this.done = true;
			return;
		}
	}

	/**
	 * Is the timer done?
	 * @return boolean
	 */
	public bool IsDone() {
		Update();
		return done;
	}

	/**
	 * How much elapsed time.
	 * @return Seconds
	 */
	public float GetProgressTime() {
		Update();
		if (this.IsDone()) {
			return countTo;
		}

		if (this.paused) {
			return PausedAt - cur;
		}

		return Time.realtimeSinceStartup - cur;
	}

	/**
	 * How far it elapsed. (0f - 1f)
	 * @return percentage
	 */
	public float GetProgress() {
		Update();
		if (this.IsDone()) {
			return 1f;
		}
		return GetProgressTime() / countTo;
	}

	/**
	 * Sets the length of time the timer should count to
	 */
	public void SetTimer(float seconds) {
		this.countTo = seconds;
	}

	float PausedAt = 0;

	/**
	 *Pauses the current timer
	 */
	public void pause() {
		if (!this.IsDone()) {
			this.paused = true;
			this.PausedAt = Time.realtimeSinceStartup;
		}
	}

	/**
	 * Resumes the current timer
	 */
	public void resume() {
		this.paused = false;
	}
}
