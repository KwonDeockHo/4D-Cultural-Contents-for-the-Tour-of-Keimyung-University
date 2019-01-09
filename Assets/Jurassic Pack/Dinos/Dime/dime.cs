using UnityEngine;

public class dime : MonoBehaviour
{
	public AudioClip Waterflush, Hit_jaw, Hit_head, Hit_tail, Medstep, Medsplash, Sniff2, Bite, Swallow, Largestep, Largesplash, Idlecarn, Dime1, Dime2, Dime3, Dime4;
	Transform Spine0, Spine1, Spine2, Spine3, Neck0, Neck1, Neck2, Head, Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8, 
	Left_Arm0, Right_Arm0, Left_Arm1, Right_Arm1, Left_Hand, Right_Hand, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot, Right_Foot;
	float crouch, spineX, spineY; bool reset;
	const float MAXYAW=30, MAXPITCH=15, MAXCROUCH=2, MAXANG=2, TANG=0.025f;

	Vector3 dir;
	shared shared;
	AudioSource[] source;
	Animator anm;
	Rigidbody body;

	//*************************************************************************************************************************************************
	//Get components
	void Start()
	{
		Left_Hips = transform.Find ("Dime/root/pelvis/left hips");
		Right_Hips = transform.Find ("Dime/root/pelvis/right hips");
		Left_Leg  = transform.Find ("Dime/root/pelvis/left hips/left leg");
		Right_Leg = transform.Find ("Dime/root/pelvis/right hips/right leg");
		
		Left_Foot = transform.Find ("Dime/root/pelvis/left hips/left leg/left foot0");
		Right_Foot = transform.Find ("Dime/root/pelvis/right hips/right leg/right foot0");
		
		Left_Arm0 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/left arm0");
		Right_Arm0 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/right arm0");
		Left_Arm1 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1");
		Right_Arm1 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1");
		
		Left_Hand = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1/left hand0");
		Right_Hand = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1/right hand0");
	
		Tail0 = transform.Find ("Dime/root/pelvis/tail0");
		Tail1 = transform.Find ("Dime/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Dime/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = transform.Find ("Dime/root/spine0");
		Spine1 = transform.Find ("Dime/root/spine0/spine1");
		Spine2 = transform.Find ("Dime/root/spine0/spine1/spine2");
		Spine3 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3");
		Neck0 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Head = transform.Find ("Dime/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/head");

		source = GetComponents<AudioSource>();
		shared= GetComponent<shared>();
		body=GetComponent<Rigidbody>();
		anm=GetComponent<Animator>();
	}

	//*************************************************************************************************************************************************
	//Check collisions
	void OnCollisionStay(Collision col) { shared.ManageCollision(col, MAXPITCH, MAXCROUCH, source, Dime3, Hit_jaw, Hit_head, Hit_tail); }

	//*************************************************************************************************************************************************
	//Play sound
	void PlaySound(string name, int time)
	{
		if((time==shared.currframe)&&(shared.currframe!=shared.lastframe))
		{
			switch (name)
			{
			case "Step": source[1].pitch=Random.Range(0.75f, 1.25f); 
				if(shared.IsOnWater) source[1].PlayOneShot(Medsplash, Random.Range(0.25f, 0.5f));
				else if(shared.IsInWater) source[1].PlayOneShot(Waterflush, Random.Range(0.25f, 0.5f));
				else source[1].PlayOneShot(Medstep, Random.Range(0.25f, 0.5f));
				shared.lastframe=shared.currframe; break;
			case "Bite": source[1].pitch=Random.Range(0.75f, 1.0f); source[1].PlayOneShot(Bite, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Die": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(shared.IsOnWater?Largesplash:Largestep, 1.0f);
				shared.lastframe=shared.currframe; shared.IsDead=true; break;
			case "Eat": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Swallow, 0.75f);
				shared.lastframe=shared.currframe; break;
			case "Sniff": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Sniff2, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sleep": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Idlecarn, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Atk": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Dime2, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "Growl": int rnd2 = Random.Range (0, 3); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd2==0)source[0].PlayOneShot(Dime1, 1.0f);
				if(rnd2==1)source[0].PlayOneShot(Dime3, 1.0f);
				else source[0].PlayOneShot(Dime4, 1.0f);
				shared.lastframe=shared.currframe; break;
			}
		}
	}

	//*************************************************************************************************************************************************
	//Motion and sound
	void FixedUpdate ()
	{
		if(!shared.IsActive) { body.Sleep(); return; }
		reset=false; shared.IsAttacking=false; shared.IsConstrained= false;
		AnimatorStateInfo CurrAnm=anm.GetCurrentAnimatorStateInfo(0);
		AnimatorStateInfo NextAnm=anm.GetNextAnimatorStateInfo(0);

		//Set Y position
		if(shared.IsOnGround)
		{ 
			body.mass=1; body.drag=4; body.angularDrag=4; anm.speed=shared.AnimSpeed; dir=transform.forward; 
			body.AddForce(Vector3.up*(shared.posY-transform.position.y)*64);
		}
		else if(shared.IsInWater && shared.Health!=0)
		{
			body.mass=5; body.drag=1; body.angularDrag=1; anm.speed=shared.AnimSpeed/2; anm.Play("Dime|Run");
			body.AddForce(Vector3.up*(shared.posY-(transform.position.y+shared.scale))*64); shared.Health-=0.1f;
		}
		else body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 128, 1.0f));

		//Stopped
		if(NextAnm.IsName("Dime|Idle1A") | NextAnm.IsName("Dime|Idle2A") | CurrAnm.IsName("Dime|Idle1A") | CurrAnm.IsName("Dime|Idle2A") |
			CurrAnm.IsName("Dime|Die1") | CurrAnm.IsName("Dime|Die2"))
		{
			if(CurrAnm.IsName("Dime|Die1")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Atk", 1); PlaySound("Die", 12); } }
			else if(CurrAnm.IsName("Dime|Die2")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Atk", 1); PlaySound("Die", 12); } }
		}

		//Forward
		else if(NextAnm.IsName("Dime|Walk") | CurrAnm.IsName("Dime|Walk") | CurrAnm.IsName("Dime|WalkGrowl") |
		 NextAnm.IsName("Dime|Step1") | CurrAnm.IsName("Dime|Step1") | NextAnm.IsName("Dime|Step2") | CurrAnm.IsName("Dime|Step2") |
		 NextAnm.IsName("Dime|ToIdle1C") | CurrAnm.IsName("Dime|ToIdle1C") | NextAnm.IsName("Dime|StepAtk1") |
		 CurrAnm.IsName("Dime|StepAtk1") | NextAnm.IsName("Dime|StepAtk2") | CurrAnm.IsName("Dime|StepAtk2"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*18*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Dime|WalkGrowl")) { PlaySound("Growl", 2); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Dime|Walk")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(NextAnm.IsName("Dime|StepAtk1") | CurrAnm.IsName("Dime|StepAtk1") | NextAnm.IsName("Dime|StepAtk2") | CurrAnm.IsName("Dime|StepAtk2"))
			{ shared.IsAttacking=true; PlaySound("Atk", 2); PlaySound("Bite", 4); } else PlaySound("Step", 8);
		}

		//Running
		else if(NextAnm.IsName("Dime|Run") | CurrAnm.IsName("Dime|Run") | CurrAnm.IsName("Dime|RunGrowl") |
		  	 NextAnm.IsName("Dime|WalkAtk") | CurrAnm.IsName("Dime|WalkAtk"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*60*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Dime|WalkAtk")) { shared.IsAttacking=true; PlaySound("Atk", 2); PlaySound("Bite", 4); }
			else if(CurrAnm.IsName("Dime|RunGrowl")) { PlaySound("Growl", 2); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Dime|Run")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else PlaySound("Step", 8);
		}
		
		//Backward
		else if(NextAnm.IsName("Dime|Step1-") | CurrAnm.IsName("Dime|Step1-") | NextAnm.IsName("Dime|Step2-") | CurrAnm.IsName("Dime|Step2-") |
		   NextAnm.IsName("Dime|ToSleep2") | CurrAnm.IsName("Dime|ToSleep2") | NextAnm.IsName("Dime|ToIdle2C") | CurrAnm.IsName("Dime|ToIdle2C") |
		   NextAnm.IsName("Dime|ToEatA") | CurrAnm.IsName("Dime|ToEatA") | NextAnm.IsName("Dime|ToEatC") | CurrAnm.IsName("Dime|ToEatC"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*-15*transform.localScale.x*anm.speed);
			PlaySound("Step", 8);
		}

		//Strafe/Turn right
		else if(CurrAnm.IsName("Dime|Strafe1-") | CurrAnm.IsName("Dime|Strafe2+"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*8*transform.localScale.x*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 13);
		}

		//Strafe/Turn left
		else if(CurrAnm.IsName("Dime|Strafe1+") | CurrAnm.IsName("Dime|Strafe2-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*-8*transform.localScale.x*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 13);
		}

		//Various
		else if(CurrAnm.IsName("Dime|EatA")) { reset=true; PlaySound("Eat", 4); }
		else if(CurrAnm.IsName("Dime|EatC")) reset=true;
		else if(CurrAnm.IsName("Dime|Sleep")) { reset=true; shared.IsConstrained=true; PlaySound("Sleep", 2); }
		else if(CurrAnm.IsName("Dime|ToSleep1") | CurrAnm.IsName("Dime|ToSleep2")) reset=true;
		else if(CurrAnm.IsName("Dime|ToSleep-")) { shared.IsConstrained=true; PlaySound("Sniff", 2); }
		else if(CurrAnm.IsName("Dime|Idle1B")) PlaySound("Growl", 1);
		else if(CurrAnm.IsName("Dime|Idle1C")) { PlaySound("Sniff", 4); PlaySound("Sniff", 7); PlaySound("Sniff", 10);}
		else if(CurrAnm.IsName("Dime|Idle2B")) PlaySound("Growl", 1);
		else if(CurrAnm.IsName("Dime|Idle2C")) { reset=true; PlaySound("Sniff", 1); }
		else if(CurrAnm.IsName("Dime|Die1-")) { shared.IsConstrained=true; PlaySound("Growl", 3); transform.tag=("Dino"); shared.IsDead=false;}
		else if(CurrAnm.IsName("Dime|Die2-")) { shared.IsConstrained=true; PlaySound("Growl", 3); transform.tag=("Dino"); shared.IsDead=false; }
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
		
		//Jaw
		shared.jaw_T=Mathf.MoveTowards(shared.jaw_T, 0, 0.5f);
		Head.GetChild(0).transform.rotation*= Quaternion.Euler(-shared.jaw_T, 0, 0);
		
		//Spine rotation
		shared.FixedHeadPos=Head.position;
		float spineAng = -anm.GetFloat("Turn")*5; float spineZ = spineX*-spineY/24;
		Neck0.transform.rotation*= Quaternion.Euler(spineY, spineZ, -spineX+spineAng);
		Neck1.transform.rotation*= Quaternion.Euler(spineY, spineZ, -spineX+spineAng);
		Neck2.transform.rotation*= Quaternion.Euler(spineY, spineZ, -spineX+spineAng);
		Head.transform.rotation*= Quaternion.Euler(spineY, spineZ, -spineX+spineAng);
		
		Spine0.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Spine1.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Spine2.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Spine3.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Tail0.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail1.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail2.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail3.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail4.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail5.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail6.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail7.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);
		Tail8.transform.rotation*= Quaternion.Euler(0, 0, -spineAng);

		//IK feet (require "JP script extension" asset)
		shared.ConvexQuadIK(Right_Arm0, Right_Arm1, Right_Hand, Left_Arm0, Left_Arm1, Left_Hand, Right_Hips, Right_Leg, Right_Foot, Left_Hips, Left_Leg, Left_Foot);
		//Check for ground layer
		shared.GetGroundAlt(true, crouch);
	
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



