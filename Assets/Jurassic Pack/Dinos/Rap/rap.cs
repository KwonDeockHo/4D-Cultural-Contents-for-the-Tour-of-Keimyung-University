using UnityEngine;

public class rap : MonoBehaviour
{
	public AudioClip Waterflush, Hit_jaw, Hit_head, Hit_tail, Smallstep, Smallsplash, Idlecarn, Swallow, Bite, Rap1, Rap2, Rap3, Rap4, Rap5, Rap6;
	Transform Spine0, Spine1, Spine2, Spine3, Spine4, Spine5, Neck0, Neck1, Neck2, Neck3, Head, 
	Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8, Tail9, Tail10, Tail11, Arm1, Arm2, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot0, Right_Foot0, Left_Foot1, Right_Foot1;
	float crouch, spineX, spineY; bool reset;
	const float MAXYAW=16, MAXPITCH=9, MAXCROUCH=2.5f, MAXANG=4, TANG=0.15f;

	Vector3 dir;
	shared shared;
	AudioSource[] source;
	Animator anm;
	Rigidbody body;

	//*************************************************************************************************************************************************
	//Get components
	void Start()
	{
		Right_Hips = transform.Find ("Rap/root/pelvis/right leg0");
		Right_Leg = transform.Find ("Rap/root/pelvis/right leg0/right leg1");
		Right_Foot0 = transform.Find ("Rap/root/pelvis/right leg0/right leg1/right foot0");
		Right_Foot1 = transform.Find ("Rap/root/pelvis/right leg0/right leg1/right foot0/right foot1");
		Left_Hips = transform.Find ("Rap/root/pelvis/left leg0");
		Left_Leg = transform.Find ("Rap/root/pelvis/left leg0/left leg1");
		Left_Foot0 = transform.Find ("Rap/root/pelvis/left leg0/left leg1/left foot0");
		Left_Foot1 = transform.Find ("Rap/root/pelvis/left leg0/left leg1/left foot0/left foot1");
	
		Tail0 = transform.Find ("Rap/root/pelvis/tail0");
		Tail1 = transform.Find ("Rap/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Tail9 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9");
		Tail10 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9/tail10");
		Tail11 = transform.Find ("Rap/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8/tail9/tail10/tail11");
		Spine0 = transform.Find ("Rap/root/spine0");
		Spine1 = transform.Find ("Rap/root/spine0/spine1");
		Spine2 = transform.Find ("Rap/root/spine0/spine1/spine2");
		Spine3 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3");
		Spine4 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4");
		Spine5 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5");
		Arm1  = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/left arm0");
		Arm2  = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/right arm0");
		Neck0 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0");
		Neck1 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1");
		Neck2 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2");
		Neck3 = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2/neck3");
		Head  = transform.Find ("Rap/root/spine0/spine1/spine2/spine3/spine4/spine5/neck0/neck1/neck2/neck3/head");
	
		source = GetComponents<AudioSource>();
		shared= GetComponent<shared>();
		body=GetComponent<Rigidbody>();
		anm=GetComponent<Animator>();
	}

	//***********************************************************************************************************************************************************************************************************
	//Check collisions
	void OnCollisionStay(Collision col) { shared.ManageCollision(col, MAXPITCH, MAXCROUCH, source, Rap1, Hit_jaw, Hit_head, Hit_tail); }

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
				else source[1].PlayOneShot(Smallstep, Random.Range(0.25f, 0.5f));
				shared.lastframe=shared.currframe; break;
			case "Bite": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(Bite, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Die": source[1].pitch=Random.Range(0.8f, 1.0f); source[1].PlayOneShot(shared.IsOnWater?Smallsplash:Smallstep, 1.0f);
				shared.lastframe=shared.currframe; shared.IsDead=true; break;
			case "Eat": source[0].pitch=Random.Range(3.0f, 3.5f); source[0].PlayOneShot(Swallow, 0.025f);
				shared.lastframe=shared.currframe; break;
			case "Sleep": source[0].pitch=Random.Range(3.0f, 3.5f); source[0].PlayOneShot(Idlecarn, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Call": source[0].pitch=Random.Range(0.9F, 1.1F); source[0].PlayOneShot(Rap4, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "AtkA": int rnd1 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd1==0)source[0].PlayOneShot(Rap2, 1.0f);
				else source[0].PlayOneShot(Rap3, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "AtkB": int rnd2 = Random.Range (0, 3); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd2==0)source[0].PlayOneShot(Rap1, 1.0f);
				else if(rnd2==1)source[0].PlayOneShot(Rap6, 1.0f);
				else source[0].PlayOneShot(Rap4, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "Growl": int rnd3 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd3==0)source[0].PlayOneShot(Rap5, 1.0f);
				else source[0].PlayOneShot(Rap6, 1.0f);
				shared.lastframe=shared.currframe; break;
			}
		}
	}
	
	//*************************************************************************************************************************************************
	//Motion and sound
	void FixedUpdate()
	{
		if(!shared.IsActive) { body.Sleep(); return; }
		reset=false; shared.IsAttacking=false; shared.IsConstrained= false;
		AnimatorStateInfo CurrAnm=anm.GetCurrentAnimatorStateInfo(0);
		AnimatorStateInfo NextAnm=anm.GetNextAnimatorStateInfo(0);

		//Set Y position
		if(shared.IsOnGround)
		{ 
			body.mass=1; body.drag=4; body.angularDrag=4; anm.speed=shared.AnimSpeed;
			body.AddForce(Vector3.up*(shared.posY-transform.position.y)*64); dir=transform.forward; anm.SetBool("OnGround", true);
		}
		else if(shared.IsInWater && shared.Health!=0)
		{
			body.mass=5; body.drag=1; body.angularDrag=1; anm.speed=shared.AnimSpeed/2; anm.Play("Rap|Run"); shared.Health-=0.1f;
			body.AddForce(Vector3.up*(shared.posY-(transform.position.y+shared.scale))*64); anm.SetBool("OnGround", true);
		}
		else { body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 64, 1.0f)); anm.SetBool("OnGround", false); }

		//Stopped
		if(NextAnm.IsName("Rap|IdleA") | CurrAnm.IsName("Rap|IdleA") | CurrAnm.IsName("Rap|Die"))
		{
			if(CurrAnm.IsName("Rap|Die")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("AtkB", 2); PlaySound("Die", 12); } }
		}

		//Jump
		else if(CurrAnm.IsName("Rap|IdleJumpStart") | CurrAnm.IsName("Rap|RunJumpStart") | CurrAnm.IsName("Rap|JumpIdle") |
			CurrAnm.IsName("Rap|IdleJumpEnd") | CurrAnm.IsName("Rap|RunJumpEnd") | CurrAnm.IsName("Rap|JumpAtk"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			if(CurrAnm.IsName("Rap|IdleJumpStart") | CurrAnm.IsName("Rap|RunJumpStart"))
			{
				PlaySound("Step", 1); PlaySound("Step", 2);
				if(CurrAnm.normalizedTime > 0.4) body.AddForce(Vector3.up*260*transform.localScale.x); 
				if(CurrAnm.IsName("Rap|RunJumpStart")) body.AddForce(dir*160*transform.localScale.x*anm.speed);
			}
			else if(CurrAnm.IsName("Rap|IdleJumpEnd") | CurrAnm.IsName("Rap|RunJumpEnd"))
			{ 
				body.drag=4; body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
				if(CurrAnm.IsName("Rap|RunJumpEnd")) body.AddForce(transform.forward*160*transform.localScale.x*anm.speed);
				 PlaySound("Step", 3); PlaySound("Step", 4); 
			}
			else { body.drag=0.1f; if(CurrAnm.IsName("Rap|JumpAtk")) { shared.IsAttacking=true; PlaySound("AtkB", 1); PlaySound("Bite", 9); } }
		}

		//Forward
		else if(NextAnm.IsName("Rap|Walk") | CurrAnm.IsName("Rap|Walk") | CurrAnm.IsName("Rap|WalkGrowl"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*32*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Rap|Walk")) { PlaySound("Step", 6); PlaySound("Step", 14);}
			else if(CurrAnm.IsName("Rap|WalkGrowl")) { PlaySound("Growl", 2); PlaySound("Step", 6); PlaySound("Step", 14); }
		}

		//Running
		else if(NextAnm.IsName("Rap|Run") | CurrAnm.IsName("Rap|Run") |
		   CurrAnm.IsName("Rap|RunGrowl") | CurrAnm.IsName("Rap|RunAtk1") |
		   (CurrAnm.IsName("Rap|RunAtk2") && CurrAnm.normalizedTime < 0.9) |
		   (CurrAnm.IsName("Rap|IdleAtk3") && CurrAnm.normalizedTime > 0.5 && CurrAnm.normalizedTime < 0.9))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*160*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Rap|Run")){ PlaySound("Step", 4); PlaySound("Step", 12); }
			else if(CurrAnm.IsName("Rap|RunGrowl")) { PlaySound("AtkB", 2); PlaySound("Step", 4); PlaySound("Step", 12); }
			else if( CurrAnm.IsName("Rap|RunAtk1")) { shared.IsAttacking=true; PlaySound("AtkB", 2); PlaySound("Step", 4); PlaySound("Step", 12); }
			else if( CurrAnm.IsName("Rap|RunAtk2")| CurrAnm.IsName("Rap|IdleAtk3"))
			{ shared.IsAttacking=true; PlaySound("AtkA", 2); PlaySound("Step", 4); PlaySound("Bite", 9); PlaySound("Step", 12); }
		}
		
		//Backward
		else if(NextAnm.IsName("Rap|Walk-") | NextAnm.IsName("Rap|WalkGrowl-") |
					CurrAnm.IsName("Rap|Walk-") | CurrAnm.IsName("Rap|WalkGrowl-"))
		{
			if(CurrAnm.normalizedTime > 0.25 && CurrAnm.normalizedTime < 0.45| 
			 CurrAnm.normalizedTime > 0.75 && CurrAnm.normalizedTime < 0.9)
			{
				body.AddForce(transform.forward*-32*transform.localScale.x*anm.speed);
				transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			}
			if(CurrAnm.IsName("Rap|WalkGrowl-")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else { PlaySound("Step", 6); PlaySound("Step", 13); }
		}

		//Strafe/Turn right
		else if(NextAnm.IsName("Rap|Strafe-") | CurrAnm.IsName("Rap|Strafe-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*16*transform.localScale.x*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 14);
		}
		
		//Strafe/Turn left
		else if(NextAnm.IsName("Rap|Strafe+") | CurrAnm.IsName("Rap|Strafe+"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*-16*transform.localScale.x*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 14);
		}

		//Various
		else if(CurrAnm.IsName("Rap|IdleAtk3")) { shared.IsAttacking=true; transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0); PlaySound("AtkA", 1); }
		else if(CurrAnm.IsName("Rap|GroundAtk")) { shared.IsAttacking=true; PlaySound("AtkB", 2); PlaySound("Bite", 4); }
		else if(CurrAnm.IsName("Rap|IdleAtk1") | CurrAnm.IsName("Rap|IdleAtk2"))
		{ shared.IsAttacking=true; transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0); PlaySound("AtkB", 2); PlaySound("Bite", 9); }
		else if(CurrAnm.IsName("Rap|ToSleep")) { reset=true; shared.IsConstrained=true; }
		else if(CurrAnm.IsName("Rap|Sleep")) { reset=true; PlaySound("Sleep", 1); shared.IsConstrained=true; }
		else if(CurrAnm.IsName("Rap|EatA")) { reset=true; PlaySound("Eat", 1); }
		else if(CurrAnm.IsName("Rap|EatB")) { reset=true; PlaySound("Bite", 3); }
		else if(CurrAnm.IsName("Rap|EatC")) reset=true;
		else if(CurrAnm.IsName("Rap|IdleC")) PlaySound("Growl", 1);
		else if(CurrAnm.IsName("Rap|IdleD")) { PlaySound("Call", 1); PlaySound("Call", 4); PlaySound("Call", 8); }
		else if(CurrAnm.IsName("Rap|IdleE")) { reset=true; PlaySound("Bite", 4); PlaySound("Bite", 7); PlaySound("Bite", 9); }
		else if(CurrAnm.IsName("Rap|Die-")) { reset=true; PlaySound("AtkA", 1); transform.tag=("Dino"); shared.IsDead=false; }
	}

	void LateUpdate()
	{
		//*************************************************************************************************************************************************
		// Bone rotation
		if(!shared.IsActive) return;
		//Reset
		if(reset)
		{
			anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), 0.0f, TANG)); crouch=Mathf.Lerp(crouch, 0, TANG);
			spineX = Mathf.Lerp(spineX, 0.0f, TANG); spineY = Mathf.Lerp(spineY, 0.0f, TANG); 
		}
		else
		{
			spineX = Mathf.LerpAngle(spineX, shared.spineX_T, TANG/6); spineY = Mathf.Lerp(spineY, shared.spineY_T, TANG/6);
			crouch=Mathf.Lerp(crouch, shared.crouch_T, TANG);
		}
		
		//Jaw
		shared.jaw_T=Mathf.MoveTowards(shared.jaw_T, 0, 0.5f);
		Head.GetChild(0).transform.rotation*= Quaternion.Euler(shared.jaw_T, 0, 0);

		//Spine rotation
		shared.FixedHeadPos=Head.position;
		float spineZ = spineX*spineY/24; float spineAng = -anm.GetFloat("Turn");
		Spine0.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Spine1.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Spine2.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Spine3.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Spine4.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Spine5.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		
		Neck0.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Neck1.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Neck2.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Neck3.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		Head.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX+spineAng);
		
		Tail0.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail1.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail2.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail3.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail4.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail5.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail6.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail7.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail8.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail9.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail10.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail11.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		
		//Arms rotation
		Arm1.transform.rotation*= Quaternion.Euler(spineY*8, 0, 0);
		Arm2.transform.rotation*= Quaternion.Euler(0, spineY*8, 0);

		//IK feet (require "JP script extension" asset)
		shared.SmallBipedIK(Right_Hips, Right_Leg, Right_Foot0, Right_Foot1, Left_Hips, Left_Leg, Left_Foot0, Left_Foot1);
		//Check for ground layer
		shared.GetGroundAlt(false, crouch);

		//*************************************************************************************************************************************************
		// CPU (require "JP script extension" asset)
		if(shared.AI && shared.Health!=0) { shared.BaseAI(Head.transform.position, MAXYAW, MAXPITCH, MAXCROUCH, MAXANG, TANG, 1, 2, 3, 4, 5, 6, 7); }
		//*************************************************************************************************************************************************
		// Human
		else if(shared.Health!=0) { shared.GetUserInputs(MAXYAW, MAXPITCH, MAXCROUCH, MAXANG, TANG, 1, 2, 3, 4, 5, 6, 7); }
		//*************************************************************************************************************************************************
		//Dead
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }
	}

}