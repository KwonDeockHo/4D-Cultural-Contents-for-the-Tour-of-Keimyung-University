using UnityEngine;

public class rex : MonoBehaviour
{
	public AudioClip Waterflush, Hit_jaw, Hit_head, Hit_tail, Bigstep, Largesplash, Largestep, Idlecarn, Bite, Swallow, Sniff1, Rex1, Rex2, Rex3, Rex4, Rex5;
	Transform Spine0, Spine1, Spine2, Neck0, Neck1, Neck2, Head, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot0, Right_Foot0, Left_Foot1, Right_Foot1;
	float crouch, spineX, spineY; bool reset;
	const float MAXYAW=25, MAXPITCH=12, MAXCROUCH=7, MAXANG=2, TANG=0.075f;

	Vector3 dir;
	shared shared;
	AudioSource[] source;
	Animator anm;
	Rigidbody body;

	//*************************************************************************************************************************************************
	//Get components
	void Start()
	{
		Right_Hips = transform.Find ("Rex/root/tail0/tail1/right hips");
		Right_Leg = transform.Find ("Rex/root/tail0/tail1/right hips/right leg");
		Right_Foot0 = transform.Find ("Rex/root/tail0/tail1/right hips/right leg/right foot0");
		Right_Foot1 = transform.Find ("Rex/root/tail0/tail1/right hips/right leg/right foot0/right foot1");
		Left_Hips = transform.Find ("Rex/root/tail0/tail1/left hips");
		Left_Leg = transform.Find ("Rex/root/tail0/tail1/left hips/left leg");
		Left_Foot0 = transform.Find ("Rex/root/tail0/tail1/left hips/left leg/left foot0");
		Left_Foot1 = transform.Find ("Rex/root/tail0/tail1/left hips/left leg/left foot0/left foot1");

		Tail1 = transform.Find ("Rex/root/tail0/tail1");
		Tail2 = transform.Find ("Rex/root/tail0/tail1/tail2");
		Tail3 = transform.Find ("Rex/root/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Rex/root/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Rex/root/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Rex/root/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Spine0 = transform.Find ("Rex/root/spine0");
		Spine1 = transform.Find ("Rex/root/spine0/spine1");
		Spine2 = transform.Find ("Rex/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Rex/root/spine0/spine1/spine2/spine3/neck0");
		Neck1 = transform.Find ("Rex/root/spine0/spine1/spine2/spine3/neck0/neck1");
		Neck2 = transform.Find ("Rex/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2");
		Head = transform.Find ("Rex/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2/head");

		source = GetComponents<AudioSource>();
		shared= GetComponent<shared>();
		body=GetComponent<Rigidbody>();
		anm=GetComponent<Animator>();
	}
	
	//***********************************************************************************************************************************************************************************************************
	//Check collisions
	void OnCollisionStay(Collision col) { shared.ManageCollision(col, MAXPITCH, MAXCROUCH, source, Rex3, Hit_jaw, Hit_head, Hit_tail); }
	

	//*************************************************************************************************************************************************
	//Play sound
	void PlaySound(string name, int time)
	{
		if((time==shared.currframe)&&(shared.currframe!=shared.lastframe))
		{
			switch (name)
			{
			case "Step": source[1].pitch=Random.Range(0.75f, 1.25f);
				if(shared.IsOnWater) source[1].PlayOneShot(Largesplash, Random.Range(0.25f, 0.5f));
				else if(shared.IsInWater) source[1].PlayOneShot(Waterflush, Random.Range(0.25f, 0.5f));
				else source[1].PlayOneShot(Bigstep, Random.Range(0.25f, 0.5f));
				shared.lastframe=shared.currframe; break;
			case "Bite": source[1].pitch=Random.Range(0.5f, 0.75f); source[1].PlayOneShot(Bite, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Die": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(shared.IsOnWater?Largesplash:Largestep, 1.0f);
				shared.lastframe=shared.currframe; shared.IsDead=true; break; 
			case "Eat": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Swallow, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sniff": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Sniff1, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sleep": source[0].pitch=Random.Range(0.75f, 1.25f); source[0].PlayOneShot(Idlecarn, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Atk": int rnd1 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd1==0)source[0].PlayOneShot(Rex3, 1.0f);
				else source[0].PlayOneShot(Rex4, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "Growl": int rnd2 = Random.Range (0, 3); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd2==0)source[0].PlayOneShot(Rex1, 1.0f);
				else if(rnd2==1) source[0].PlayOneShot(Rex2, 1.0f);
				else source[0].PlayOneShot(Rex5, 1.0f);
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
			body.mass=5; body.drag=1; body.angularDrag=1; anm.speed=shared.AnimSpeed/2; anm.Play("Rex|Run");
			body.AddForce(Vector3.up*(shared.posY-(transform.position.y+shared.scale))*64); shared.Health-=0.1f;
		}
		else body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 128, 1.0f));


		//Stopped
		if(NextAnm.IsName("Rex|Idle1A") | NextAnm.IsName("Rex|Idle2A") | CurrAnm.IsName("Rex|Idle1A") | CurrAnm.IsName("Rex|Idle2A") |
			CurrAnm.IsName("Rex|Die1") | CurrAnm.IsName("Rex|Die2"))
		{
			if(CurrAnm.IsName("Rex|Die1")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Atk", 2); PlaySound("Die", 12); } }
			else if(CurrAnm.IsName("Rex|Die2")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Atk", 2); PlaySound("Die", 10); } }
		}

		//End Forward
		else if((CurrAnm.IsName("Rex|Step1+") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|Step2+") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|ToIdle1C") && CurrAnm.normalizedTime > 0.5) | 
		 (CurrAnm.IsName("Rex|ToIdle2B") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|ToIdle2D") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|ToEatA") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|ToEatC") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|StepAtk1") && CurrAnm.normalizedTime > 0.5) |
		 (CurrAnm.IsName("Rex|StepAtk2") && CurrAnm.normalizedTime > 0.5))
		{
			PlaySound("Step", 9);
			if(CurrAnm.IsName("Rex|StepAtk1") | CurrAnm.IsName("Rex|StepAtk2")) shared.IsAttacking=true;
		}

		//Forward
		else if(CurrAnm.IsName("Rex|Walk") | CurrAnm.IsName("Rex|WalkGrowl") |
		   (CurrAnm.IsName("Rex|Step1+") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Rex|Step2+") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Rex|ToIdle2B") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Rex|ToIdle1C") && CurrAnm.normalizedTime < 0.5) | 
		   (CurrAnm.IsName("Rex|ToIdle2D") && CurrAnm.normalizedTime < 05) |
		   (CurrAnm.IsName("Rex|ToEatA") && CurrAnm.normalizedTime < 0.5) |
		   (CurrAnm.IsName("Rex|ToEatC") && CurrAnm.normalizedTime < 0.5))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*48*transform.localScale.x*anm.speed);
			if(anm.GetCurrentAnimatorStateInfo(0).IsName("Rex|WalkGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(anm.GetCurrentAnimatorStateInfo(0).IsName("Rex|Walk")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else { PlaySound("Step", 8); PlaySound("Step", 12); }
		}

		//Run
		else if(CurrAnm.IsName("Rex|Run") | CurrAnm.IsName("Rex|RunGrowl") |
		   CurrAnm.IsName("Rex|WalkAtk1") | CurrAnm.IsName("Rex|WalkAtk2") |
		   (CurrAnm.IsName("Rex|StepAtk1") && CurrAnm.normalizedTime < 0.6) |
		   (CurrAnm.IsName("Rex|StepAtk2") && CurrAnm.normalizedTime < 0.6))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*128*transform.localScale.x*anm.speed);
			if(anm.GetCurrentAnimatorStateInfo(0).IsName("Rex|RunGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(anm.GetCurrentAnimatorStateInfo(0).IsName("Rex|Run")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Rex|StepAtk1") | CurrAnm.IsName("Rex|StepAtk2")) { shared.IsAttacking=true; PlaySound("Atk", 2); PlaySound("Bite", 5); }
			else { shared.IsAttacking=true; PlaySound("Atk", 2); PlaySound("Step", 6); PlaySound("Bite", 10); PlaySound("Step", 13); }
		}

		//Backward
		else if((CurrAnm.IsName("Rex|Step1-") && CurrAnm.normalizedTime < 0.8) |
		   (CurrAnm.IsName("Rex|Step2-") && CurrAnm.normalizedTime < 0.8) |
		   (CurrAnm.IsName("Rex|ToSleep2")&& CurrAnm.normalizedTime < 0.8))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*-48*transform.localScale.x*anm.speed);
			PlaySound("Step", 12);
		}

		//Strafe/Turn right
		else if(CurrAnm.IsName("Rex|Strafe1-") | CurrAnm.IsName("Rex|Strafe2+"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*25*transform.localScale.x*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 13);
		}

		//Strafe/Turn left
		else if(CurrAnm.IsName("Rex|Strafe1+") | CurrAnm.IsName("Rex|Strafe2-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*-25*transform.localScale.x*anm.speed);
			PlaySound("Step", 6); PlaySound("Step", 13);
		}

		//Various
		else if(CurrAnm.IsName("Rex|EatA")) { reset=true; PlaySound("Eat", 4); PlaySound("Bite", 5); }
		else if(CurrAnm.IsName("Rex|EatB") | CurrAnm.IsName("Rex|EatC")) reset=true;
		else if(CurrAnm.IsName("Rex|Sleep")) { reset=true; shared.IsConstrained=true; PlaySound("Sleep", 2); }
		else if(CurrAnm.IsName("Rex|ToSleep1") | CurrAnm.IsName("Rex|ToSleep2")) { reset=true; shared.IsConstrained=true; }
		else if(CurrAnm.IsName("Rex|ToIdle2A")) PlaySound("Sniff", 1);
		else if(CurrAnm.IsName("Rex|Idle1B")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Rex|Idle1C")) { PlaySound("Sniff", 4); PlaySound("Sniff", 7); PlaySound("Sniff", 10);}
		else if(CurrAnm.IsName("Rex|Idle2B")) { reset=true; PlaySound("Bite", 4); PlaySound("Bite", 6); PlaySound("Bite", 8);}
		else if(CurrAnm.IsName("Rex|Idle2C")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Rex|Idle2D")) { reset=true; PlaySound("Atk", 2); }
		else if(CurrAnm.IsName("Rex|IdleAtk1") | CurrAnm.IsName("Rex|IdleAtk2"))
		{ shared.IsAttacking=true; transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0); PlaySound("Atk", 1); PlaySound("Step", 3); PlaySound("Bite", 6); } 
		else if(CurrAnm.IsName("Rex|Die1-")) { shared.IsConstrained=true; PlaySound("Growl", 3); transform.tag=("Dino"); shared.IsDead=false; }
		else if(CurrAnm.IsName("Rex|Die2-")) { shared.IsConstrained=true; PlaySound("Growl", 3); transform.tag=("Dino"); shared.IsDead=false; }
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
		float spineZ = spineX*spineY/24; float spineAng = anm.GetFloat("Turn")*5;
		Spine0.transform.rotation*= Quaternion.Euler(-spineY, 0, 0);
		Spine1.transform.rotation*= Quaternion.Euler(-spineY, 0, -spineX-spineAng);
		Spine2.transform.rotation*= Quaternion.Euler(-spineY, 0, -spineX-spineAng);
		Neck0.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX-spineAng);
		Neck1.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX-spineAng);
		Neck2.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX-spineAng);
		Head.transform.rotation*= Quaternion.Euler(-spineY, spineZ, -spineX-spineAng);

		Tail1.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Tail2.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Tail3.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Tail4.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Tail5.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Tail6.transform.rotation*= Quaternion.Euler(0, 0, spineAng);

		//IK feet (require "JP script extension" asset)
		shared.LargeBipedIK(Right_Hips, Right_Leg, Right_Foot0, Right_Foot1, Left_Hips, Left_Leg, Left_Foot0, Left_Foot1);
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

