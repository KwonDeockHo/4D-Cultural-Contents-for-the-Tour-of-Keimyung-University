using UnityEngine;

public class pachy : MonoBehaviour
{
	public AudioClip Waterflush, Hit_jaw, Hit_head, Hit_tail, Medstep, Medsplash, Sniff2, Chew, Idleherb, Pachy1, Pachy2, Pachy3, Pachy4;
	Transform Spine0, Spine1, Spine2, Neck0, Neck1, Neck2, Head, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot0, Right_Foot0, Left_Foot1, Right_Foot1;
	float crouch, spineX, spineY; bool reset;
	const float MAXYAW=25, MAXPITCH=12, MAXCROUCH=6, MAXANG=3, TANG=0.1f;

	Vector3 dir;
	shared shared;
	AudioSource[] source;
	Animator anm;
	Rigidbody body;

//*************************************************************************************************************************************************
	//Get components
	void Start()
	{
		Right_Hips = transform.Find ("Pachy/root/tail0/tail1/right hips");
		Right_Leg = transform.Find ("Pachy/root/tail0/tail1/right hips/right leg");
		Right_Foot0 = transform.Find ("Pachy/root/tail0/tail1/right hips/right leg/right foot0");
		Right_Foot1 = transform.Find ("Pachy/root/tail0/tail1/right hips/right leg/right foot0/right foot1");
		Left_Hips = transform.Find ("Pachy/root/tail0/tail1/left hips");
		Left_Leg = transform.Find ("Pachy/root/tail0/tail1/left hips/left leg");
		Left_Foot0 = transform.Find ("Pachy/root/tail0/tail1/left hips/left leg/left foot0");
		Left_Foot1 = transform.Find ("Pachy/root/tail0/tail1/left hips/left leg/left foot0/left foot1");
	
		Tail1 = transform.Find ("Pachy/root/tail0/tail1");
		Tail2 = transform.Find ("Pachy/root/tail0/tail1/tail2");
		Tail3 = transform.Find ("Pachy/root/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Pachy/root/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Pachy/root/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Pachy/root/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Spine0 = transform.Find ("Pachy/root/spine0");
		Spine1 = transform.Find ("Pachy/root/spine0/spine1");
		Spine2 = transform.Find ("Pachy/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Pachy/root/spine0/spine1/spine2/spine3/neck0");
		Neck1 = transform.Find ("Pachy/root/spine0/spine1/spine2/spine3/neck0/neck1");
		Neck2 = transform.Find ("Pachy/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2");
		Head = transform.Find ("Pachy/root/spine0/spine1/spine2/spine3/neck0/neck1/neck2/head");

		source = GetComponents<AudioSource>();
		shared= GetComponent<shared>();
		body=GetComponent<Rigidbody>();
		anm=GetComponent<Animator>();
	}

	//*************************************************************************************************************************************************
	//Check collisions
	void OnCollisionStay(Collision col) { shared.ManageCollision(col, MAXPITCH, MAXCROUCH, source, Pachy3, Hit_jaw, Hit_head, Hit_tail); }
	
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
			case "Die": source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(shared.IsOnWater?Medsplash:Medstep, 1.0f);
				shared.lastframe=shared.currframe; shared.IsDead=true; break;
			case "Chew": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Chew, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sniff": source[0].pitch=Random.Range(1.25f, 1.5f); source[0].PlayOneShot(Sniff2, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sleep": source[0].pitch=Random.Range(1.0f, 1.25f); source[0].PlayOneShot(Idleherb, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Atk": int rnd1 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd1==0)source[0].PlayOneShot(Pachy2, 1.0f);
				else source[0].PlayOneShot(Pachy4, 1.0f);
				shared.lastframe=shared.currframe; break;
			case "Growl": int rnd2 = Random.Range (0, 2); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd2==0)source[0].PlayOneShot(Pachy1, 1.0f);
				else source[0].PlayOneShot(Pachy3, 1.0f);
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
			body.mass=5; body.drag=1; body.angularDrag=1; anm.speed=shared.AnimSpeed/2; anm.Play("Pachy|Run");
			body.AddForce(Vector3.up*(shared.posY-(transform.position.y+shared.scale))*64); shared.Health-=0.1f;
		}
		else body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 128, 1.0f));

		//Stopped
		if(NextAnm.IsName("Pachy|IdleA") | CurrAnm.IsName("Pachy|IdleA") | CurrAnm.IsName("Pachy|Die"))
		{
			if(CurrAnm.IsName("Pachy|Die")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Atk", 1); PlaySound("Die", 12); } }
		}

		//Forward
		else if(NextAnm.IsName("Pachy|Walk") | CurrAnm.IsName("Pachy|Walk") | CurrAnm.IsName("Pachy|WalkGrowl"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*48*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Pachy|WalkGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else { PlaySound("Step", 6); PlaySound("Step", 13); }
		}

		//Running
		else if(NextAnm.IsName("Pachy|Run") | CurrAnm.IsName("Pachy|Run") | CurrAnm.IsName("Pachy|RunGrowl") |
		   (CurrAnm.IsName("Pachy|RunAtk") && CurrAnm.normalizedTime < 0.6))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*128*transform.localScale.x*anm.speed);
			PlaySound("Step", 4); PlaySound("Step",12); 
			if(CurrAnm.IsName("Pachy|RunGrowl")) PlaySound("Growl", 1);
			else if(CurrAnm.IsName("Pachy|RunAtk")) { shared.IsAttacking=true; PlaySound("Atk", 3); }
		}

		//Backward
		else if(NextAnm.IsName("Pachy|Walk-") | CurrAnm.IsName("Pachy|Walk-") | CurrAnm.IsName("Pachy|WalkGrowl-"))
		{	
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*-40*transform.localScale.x*anm.speed);
			PlaySound("Step", 4); PlaySound("Step",12); 
			if(CurrAnm.IsName("Pachy|WalkGrowl-")) PlaySound("Growl", 1);
		}

		//Strafe/Turn right
		else if(NextAnm.IsName("Pachy|Strafe-") | CurrAnm.IsName("Pachy|Strafe-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*20*transform.localScale.x*anm.speed);
			PlaySound("Step", 4); PlaySound("Step", 10);
		}

		//Strafe/Turn left
		else if(NextAnm.IsName("Pachy|Strafe+") | CurrAnm.IsName("Pachy|Strafe+"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*-20*transform.localScale.x*anm.speed);
			PlaySound("Step", 4); PlaySound("Step", 10);
		}

		//Various
		else if(CurrAnm.IsName("Pachy|EatA")) PlaySound("Chew", 10);
		else if(CurrAnm.IsName("Pachy|EatB")) { PlaySound("Chew", 1); PlaySound("Chew", 4); PlaySound("Chew", 8); PlaySound("Chew", 12); }
		else if(CurrAnm.IsName("Pachy|EatC")) reset=true;
		else if(CurrAnm.IsName("Pachy|ToSleep")) { reset=true; shared.IsConstrained=true; }
		else if(CurrAnm.IsName("Pachy|Sleep")) { reset=true; shared.IsConstrained=true; PlaySound("Sleep", 2); }
		else if(CurrAnm.IsName("Pachy|ToSleep-")) { shared.IsConstrained=true; PlaySound("Sniff", 1); }
		else if(CurrAnm.IsName("Pachy|IdleB")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Pachy|IdleC")) { reset=true; PlaySound("Sniff", 1); }
		else if(CurrAnm.IsName("Pachy|IdleD")) PlaySound("Growl", 2);
		else if(CurrAnm.IsName("Pachy|IdleAtk")) { shared.IsAttacking=true; PlaySound("Atk", 2); PlaySound("Step", 3); }
		else if(CurrAnm.IsName("Pachy|Die-")) { shared.IsConstrained=true; PlaySound("Growl", 3); shared.IsDead=false; transform.tag=("Dino"); }
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
			spineX = Mathf.Lerp(spineX, 0.0f, TANG/6); spineY = Mathf.Lerp(spineY, 0.0f, TANG/6); 
		}
		else
		{
			spineX = Mathf.Lerp(spineX, shared.spineX_T, TANG/6); spineY = Mathf.Lerp(spineY, shared.spineY_T, TANG/6);
			crouch=Mathf.Lerp(crouch, shared.crouch_T, TANG);
		}
		
		//Jaw
		shared.jaw_T=Mathf.MoveTowards(shared.jaw_T, 0, 0.5f);
		Head.GetChild(0).transform.rotation*= Quaternion.Euler(shared.jaw_T, 0, 0);

		//Spine rotation
		shared.FixedHeadPos=Head.position;
		float spineZ = spineX*spineY/24; float spineAng = anm.GetFloat("Turn")*8;
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
		if(shared.AI && shared.Health!=0) { shared.BaseAI(Head.transform.position, MAXYAW, MAXPITCH, MAXCROUCH, MAXANG, TANG, 1, 2, 3, 0, 4, 5, 6); }
		//*************************************************************************************************************************************************
		// Human
		else if(shared.Health!=0) { shared.GetUserInputs(MAXYAW, MAXPITCH, MAXCROUCH, MAXANG, TANG, 1, 2, 3, 0, 4, 5, 6); }
		//*************************************************************************************************************************************************
		//Dead
		else { anm.SetBool("Attack", false); anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", -1); }
	}
}
