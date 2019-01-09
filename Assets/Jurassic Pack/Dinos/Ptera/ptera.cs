using UnityEngine;
using System.Collections.Generic;

public class ptera : MonoBehaviour
{
	public AudioClip Waterflush, Wind, Hit_jaw, Hit_head, Hit_tail, Smallstep, Smallsplash, Swallow, Idlecarn, Bite, Sniff2, Bigstep, Largesplash, Ptera1, Ptera2, Ptera3;
	Transform Root, Neck0, Neck1, Neck2, Neck3, Neck4, Neck5, Neck6, Head, 
	Right_Wing0, Left_Wing0, Right_Wing1, Left_Wing1, Right_Hand, Left_Hand, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot, Right_Foot;
	float crouch, spineX, spineY, flyRoll=0, flyPitch=0; bool reset;
	const float MAXYAW=12, MAXPITCH=10, MAXCROUCH=2.5f, MAXANG=2, TANG=0.05f;

	Vector3 dir;
	shared shared;
	AudioSource[] source;
	Animator anm;
	Rigidbody body;

	//*************************************************************************************************************************************************
	//Get components
	void Start()
	{
		Root = transform.Find ("Ptera/root");

		Left_Hips = transform.Find ("Ptera/root/low spine0/low spine1/pelvis/left hips");
		Right_Hips = transform.Find ("Ptera/root/low spine0/low spine1/pelvis/right hips");
		Left_Leg  = transform.Find ("Ptera/root/low spine0/low spine1/pelvis/left hips/left leg");
		Right_Leg = transform.Find ("Ptera/root/low spine0/low spine1/pelvis/right hips/right leg");
		Left_Foot = transform.Find ("Ptera/root/low spine0/low spine1/pelvis/left hips/left leg/left foot");
		Right_Foot = transform.Find ("Ptera/root/low spine0/low spine1/pelvis/right hips/right leg/right foot");


		Right_Wing0 = transform.Find ("Ptera/root/spine0/spine1/spine2/right wing0");
		Left_Wing0 = transform.Find ("Ptera/root/spine0/spine1/spine2/left wing0");
		Right_Wing1 = transform.Find ("Ptera/root/spine0/spine1/spine2/right wing0/right wing1/right wing2");
		Left_Wing1 = transform.Find ("Ptera/root/spine0/spine1/spine2/left wing0/left wing1/left wing2");
		Right_Hand = transform.Find ("Ptera/root/spine0/spine1/spine2/right wing0/right wing1/right wing2/right wing3");
		Left_Hand = transform.Find ("Ptera/root/spine0/spine1/spine2/left wing0/left wing1/left wing2/left wing3");

		Neck0 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0");
		Neck1 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1");
		Neck2 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2");
		Neck3 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3");
		Neck4 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4");
		Neck5 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5");
		Neck6 = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6");
		Head  = transform.Find ("Ptera/root/spine0/spine1/spine2/neck0/neck1/neck2/neck3/neck4/neck5/neck6/head");

		source = GetComponents<AudioSource>();
		shared= GetComponent<shared>();
		body=GetComponent<Rigidbody>();
		anm=GetComponent<Animator>();
	}
	
	//*************************************************************************************************************************************************
	//Check collisions
	void OnCollisionStay(Collision col) { shared.ManageCollision(col, MAXPITCH, MAXCROUCH, source, Ptera3, Hit_jaw, Hit_head, Hit_tail); }

	//*************************************************************************************************************************************************
	//Play sound
	void PlaySound(string name, int time)
	{
		if((time==shared.currframe)&&(shared.currframe!=shared.lastframe))
		{
			switch (name)
			{
			case "Step": source[1].pitch=Random.Range(0.75f, 1.25f); 
				if(shared.IsOnWater) source[1].PlayOneShot(Smallsplash, Random.Range(0.25f, 0.5f));
				else if(shared.IsInWater) source[1].PlayOneShot(Waterflush, Random.Range(0.25f, 0.5f));
				else if(shared.IsOnGround) source[1].PlayOneShot(Smallstep, Random.Range(0.25f, 0.5f));
				shared.lastframe=shared.currframe; break;
			case "Bite": source[1].pitch=Random.Range(1.5f, 1.75f); source[1].PlayOneShot(Bite, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sniff": source[1].pitch=Random.Range(1.5f, 1.75f);
				if(shared.IsInWater) source[1].PlayOneShot(Waterflush, Random.Range(0.25f, 0.5f));
				else source[1].PlayOneShot(Sniff2, Random.Range(0.1f, 0.2f));
				shared.lastframe=shared.currframe; break;
			case "Die": source[1].pitch=Random.Range(0.8f, 1.0f); source[1].PlayOneShot(shared.IsOnWater?Largesplash:Bigstep, 1.0f);
				shared.lastframe=shared.currframe; shared.IsDead=true; break;
			case "Eat": source[0].pitch=Random.Range(3.0f, 3.25f); source[0].PlayOneShot(Swallow, 0.1f);
				shared.lastframe=shared.currframe; break;
			case "Sleep": source[0].pitch=Random.Range(3.0f, 3.25f); source[0].PlayOneShot(Idlecarn, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Atk": int rnd1 = Random.Range(0, 3); source[0].pitch=Random.Range(1.5f, 1.75f);
				if(rnd1==0) source[0].PlayOneShot(Ptera1, 1.0f);
				else if(rnd1==1) source[0].PlayOneShot(Ptera3, 1.0f);
				else source[0].PlayOneShot(Sniff2, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "Growl": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Ptera2, 1.0f);
				shared.lastframe=shared.currframe; break;
			}
		}
	}

	//*************************************************************************************************************************************************
	//Motion and sound
	void FixedUpdate ()
	{
		if(!shared.IsActive) { body.Sleep(); return; }
		reset=false; shared.IsAttacking=false; shared.IsFlying=false; shared.IsConstrained= false;
		AnimatorStateInfo CurrAnm=anm.GetCurrentAnimatorStateInfo(0);
		AnimatorStateInfo NextAnm=anm.GetNextAnimatorStateInfo(0);

		//Set Y position
		if(shared.IsOnGround)
		{
			body.mass=1; body.drag=4; body.angularDrag=4; anm.speed=shared.AnimSpeed;
			flyRoll = Mathf.Lerp(flyRoll, 0.0f, 0.1f); flyPitch = Mathf.Lerp(flyPitch, 0.0f, 0.1f);
			body.AddForce(Vector3.up*(shared.posY-transform.position.y)*64); dir=-Root.right;
		} 
		else if(shared.IsInWater && shared.Health!=0)
		{
			body.mass=5; body.drag=1; body.angularDrag=1; anm.speed=shared.AnimSpeed/2; anm.Play("Ptera|Run");
			flyRoll = Mathf.Lerp(flyRoll, 0.0f, 0.1f); flyPitch = Mathf.Lerp(flyPitch, 0.0f, 0.1f); shared.Health-=0.1f;
			body.AddForce(Vector3.up*(shared.posY-(transform.position.y+shared.scale))*64); anm.SetBool("OnGround", true);
		}
		else { body.mass=1; body.drag=1; body.angularDrag=1; }

		//Stopped
		if(NextAnm.IsName("Ptera|IdleA") | CurrAnm.IsName("Ptera|IdleA") | CurrAnm.IsName("Ptera|Die1") | CurrAnm.IsName("Ptera|Die2") | CurrAnm.IsName("Ptera|Fall"))
		{

			if(CurrAnm.IsName("Ptera|Die1")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Growl", 1); PlaySound("Die", 11); } }
			else if(CurrAnm.IsName("Ptera|Die2"))
			{
				reset=true; shared.IsConstrained=true; body.velocity = new Vector3(0, 0, 0); 
				if(!shared.IsDead) PlaySound("Die", 0);
			}
			else if(CurrAnm.IsName("Ptera|Fall"))
			{
				reset=true; shared.IsFlying=true;
				body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 64, 1.0f));
				if(CurrAnm.normalizedTime<0.1f) source[0].PlayOneShot(Ptera2, 1.0f);
			} 
		}
		
		//Forward
		else if(NextAnm.IsName("Ptera|Walk") | CurrAnm.IsName("Ptera|Walk"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*10*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Running
		else if(NextAnm.IsName("Ptera|Run") | CurrAnm.IsName("Ptera|Run"))
		{
			shared.IsFlying=true;
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*100*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 6); PlaySound("Sniff", 7); PlaySound("Sniff", 8);
		}
		
		//Backward
		else if(NextAnm.IsName("Ptera|Walk-") | CurrAnm.IsName("Ptera|Walk-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*-5*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}
		
		//Strafe/Turn right
		else if(NextAnm.IsName("Ptera|Strafe+") | CurrAnm.IsName("Ptera|Strafe+"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*8*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}
		
		//Strafe/Turn left
		else if(NextAnm.IsName("Ptera|Strafe-") | CurrAnm.IsName("Ptera|Strafe-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*-8*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Takeoff
		else if(CurrAnm.IsName("Ptera|Takeoff"))
		{
			shared.IsFlying=true;
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			if(CurrAnm.normalizedTime > 0.5) body.AddForce(Vector3.up*50*transform.localScale.x);
			PlaySound("Sniff", 7); PlaySound("Sniff", 8);
		}

		//Run to Fly
		else if(NextAnm.IsName("Ptera|RunToFlight") | CurrAnm.IsName("Ptera|RunToFlight"))
		{
			shared.IsFlying=true;
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce((transform.forward*100*transform.localScale.x*anm.speed) + (Vector3.up*50*transform.localScale.x));
			PlaySound("Step", 5); PlaySound("Step", 6); PlaySound("Sniff", 7); PlaySound("Sniff", 8);
		}
		
		//Fly to Run
		else if(NextAnm.IsName("Ptera|FlightToRun") | CurrAnm.IsName("Ptera|FlightToRun"))
		{
			shared.IsFlying=true;
			body.AddForce(transform.forward*100*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 6); PlaySound("Sniff", 7); PlaySound("Sniff", 8);
		}

		//Fly
		else if(NextAnm.IsName("Ptera|Flight") | CurrAnm.IsName("Ptera|Flight") | NextAnm.IsName("Ptera|FlightGrowl") | CurrAnm.IsName("Ptera|FlightGrowl") |
		   NextAnm.IsName("Ptera|Glide") | CurrAnm.IsName("Ptera|Glide") | NextAnm.IsName("Ptera|GlideGrowl") | CurrAnm.IsName("Ptera|GlideGrowl"))
		{
			shared.IsFlying=true;
			flyRoll = Mathf.Lerp(flyRoll, anm.GetFloat("Turn")*30, 0.05f); //roll root
			flyPitch = Mathf.Clamp(Mathf.Lerp(flyPitch, anm.GetFloat("Pitch")*90, 0.01f), -35f, 90f); //pitch root
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(-Root.right*(50+Mathf.Abs(anm.GetFloat("Pitch")*30))*transform.localScale.x);
			if(body.velocity.magnitude!=0) body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 64/body.velocity.magnitude, 1.0f));

			if(CurrAnm.IsName("Ptera|Flight")) { PlaySound("Sniff", 5); PlaySound("Sniff", 6); }
			else if(CurrAnm.IsName("Ptera|FlightGrowl")) { PlaySound("Atk", 2); PlaySound("Sniff", 5); }
			else if(CurrAnm.IsName("Ptera|GlideGrowl")) PlaySound("Growl", 2);
		}
		
		//Fly - Stationary
		else if(CurrAnm.IsName("Ptera|Statio") | CurrAnm.IsName("Ptera|StatioGrowl") | CurrAnm.IsName("Ptera|IdleD") | CurrAnm.IsName("Ptera|FlyAtk"))
		{
			shared.IsFlying=true;
			flyRoll = Mathf.Lerp(flyRoll, anm.GetFloat("Turn")*16, 0.1f);  flyPitch = Mathf.Lerp(flyPitch, 0.0f, 0.1f);
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0); //turn
			body.AddForce(Vector3.up*40*transform.localScale.x*-anm.GetFloat("Pitch")); //fly up/down
			if(shared.IsOnGround&&CurrAnm.IsName("Ptera|FlyAtk")) body.AddForce(Vector3.up*50*transform.localScale.x); //takeoff
			if(anm.GetInteger("Move")==1 | anm.GetInteger("Move")==2 | anm.GetInteger("Move")==3) body.AddForce(transform.forward*60*transform.localScale.x); //fly forward
			else if(anm.GetInteger("Move")== -1) body.AddForce(transform.forward*-60*transform.localScale.x); //fly backward
			else if(anm.GetInteger("Move")== -10) body.AddForce(transform.right*60*transform.localScale.x); //fly right
			else if(anm.GetInteger("Move") == 10) body.AddForce(transform.right*-60*transform.localScale.x); //fly left

			if(CurrAnm.IsName("Ptera|StatioGrowl")) PlaySound("Atk", 2);
			else if(CurrAnm.IsName("Ptera|IdleD")) { PlaySound("Atk", 2); PlaySound("Step", 10); }
			else if(CurrAnm.IsName("Ptera|FlyAtk")) { shared.IsAttacking=true; PlaySound("Atk", 3); PlaySound("Bite", 8); }
			else { PlaySound("Sniff", 5); PlaySound("Sniff", 6); }
		}
	
		//Various
		else if(CurrAnm.IsName("Ptera|Landing")) { shared.IsFlying=true; PlaySound("Step", 2); PlaySound("Step", 3); }
		else if(CurrAnm.IsName("Ptera|IdleB")) PlaySound("Atk", 2);
		else if(CurrAnm.IsName("Ptera|IdleC")) { reset=true; shared.IsConstrained=true; }
		else if(CurrAnm.IsName("Ptera|EatA")) { reset=true; PlaySound("Eat", 1); }
		else if(CurrAnm.IsName("Ptera|EatB")) { reset=true; PlaySound("Bite", 0); }
		else if(CurrAnm.IsName("Ptera|EatC")) reset=true;
		else if(CurrAnm.IsName("Ptera|ToSleep")){ reset=true; shared.IsConstrained=true; }
		else if(CurrAnm.IsName("Ptera|Sleep")) { reset=true; shared.IsConstrained=true; PlaySound("Sleep", 1); }
		else if(CurrAnm.IsName("Ptera|Die-")) { shared.IsConstrained=true; PlaySound("Atk", 2); transform.tag=("Dino"); shared.IsDead=false; }

		//Play wind sound based on speed
		if(shared.IsFlying)
		{
			if(!source[2].isPlaying) source[2].PlayOneShot(Wind);
			source[2].volume=body.velocity.magnitude/(80*transform.localScale.x);
			source[2].pitch=body.velocity.magnitude/(40*transform.localScale.x);
		}
		else if(source[2].isPlaying) source[2].Pause();
	}

	void LateUpdate()
	{
		//*************************************************************************************************************************************************
		// Bone rotation
		if(!shared.IsActive) return;
		//Reset
		if(reset)
		{
			anm.SetFloat("Turn", Mathf.Lerp(anm.GetFloat("Turn"), 0.0f, TANG)); crouch=Mathf.Lerp(crouch, 0, TANG);
			spineX = Mathf.Lerp(spineX, 0.0f, TANG); spineY = Mathf.Lerp(spineY, 0.0f, TANG); 
		}
		else
		{
			spineX = Mathf.Lerp(spineX, shared.spineX_T, TANG); spineY = Mathf.Lerp(spineY, shared.spineY_T, TANG);
			crouch=Mathf.Lerp(crouch, shared.crouch_T, TANG);
		}
		
		//Pitch/roll root
		Root.transform.rotation*= Quaternion.Euler(flyRoll, flyPitch, 0);
		
		//Wings
		Right_Wing0.transform.rotation*= Quaternion.Euler(flyRoll/2, Mathf.Clamp(flyRoll, -35, 0), Mathf.Clamp(-flyPitch, -35, 0));
		Left_Wing0.transform.rotation*= Quaternion.Euler(flyRoll/2, Mathf.Clamp(-flyRoll, -35, 0), Mathf.Clamp(flyPitch, 0, 35));
		Right_Wing0.GetChild(0).transform.rotation*= Quaternion.Euler(0, 0, Mathf.Clamp(flyPitch, 0, 90)+Mathf.Abs(flyRoll)/2);
		Left_Wing0.GetChild(0).transform.rotation*= Quaternion.Euler(0, 0, Mathf.Clamp(-flyPitch, -90, 0)-Mathf.Abs(flyRoll)/2);
		Right_Hand.transform.rotation*= Quaternion.Euler(0, 0, Mathf.Clamp(-flyPitch, -90, 0)-Mathf.Abs(flyRoll));
		Left_Hand.transform.rotation*= Quaternion.Euler(0, 0, Mathf.Clamp(flyPitch, 0, 90)+Mathf.Abs(flyRoll));
		
		//Jaw
		shared.jaw_T=Mathf.MoveTowards(shared.jaw_T, 0, 0.5f);
		Head.GetChild(0).transform.rotation*= Quaternion.Euler(0, shared.jaw_T, 0);
		
		//Spine rotation
		shared.FixedHeadPos=Head.position;
		float spineZ = spineX*-spineY/24; float spineAng = flyRoll/5;
		Neck0.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Neck1.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Neck2.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Neck3.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Neck4.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Neck5.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Neck6.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);
		Head.transform.rotation*= Quaternion.Euler(-spineZ, -spineY, -spineX-spineAng);

		//IK feet (require "JP script extension" asset)
		shared.FlyingIK( Right_Wing0, Right_Wing1, Right_Hand, Left_Wing0, Left_Wing1, Left_Hand, Right_Hips, Right_Leg, Right_Foot, Left_Hips, Left_Leg, Left_Foot);
		//Check for ground layer
		shared.GetGroundAlt(true, crouch); anm.SetBool("OnGround", shared.IsOnGround);

		//*************************************************************************************************************************************************
		// CPU (require "JP script extension" asset)
		if(shared.AI && shared.Health!=0) { shared.BaseAI(Head.transform.position, MAXYAW, MAXPITCH, MAXCROUCH, MAXANG, TANG, 1, 2, 3, 0, 4, 5, 6); }
		//*************************************************************************************************************************************************
		// Human
		else if(shared.Health!=0) { shared.GetUserInputs(MAXYAW, MAXPITCH, MAXCROUCH, MAXANG, TANG, 1, 2, 3, 0, 4, 5, 6); }
		//*************************************************************************************************************************************************
		//Dead
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }
	}
}










