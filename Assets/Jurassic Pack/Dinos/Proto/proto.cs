using UnityEngine;

public class proto : MonoBehaviour
{
	public AudioClip Waterflush, Hit_jaw, Hit_head, Hit_tail, Smallstep, Medsplash, Sniff2, Chew, Slip, Largestep, Largesplash, Idleherb, Proto1, Proto2, Proto3, Proto4;
	Transform Spine0, Spine1, Spine2, Neck0, Neck1, Neck2, Neck3, Head, Tail0, Tail1, Tail2, Tail3, Tail4, Tail5, Tail6, Tail7, Tail8, 
	Left_Arm0, Right_Arm0, Left_Arm1, Right_Arm1, Left_Hand, Right_Hand, 
	Left_Hips, Right_Hips, Left_Leg, Right_Leg, Left_Foot, Right_Foot;
	float crouch, spineX, spineY; bool reset;
	const float MAXYAW=20, MAXPITCH=15, MAXCROUCH=2, MAXANG=2, TANG=0.05f;

	Vector3 dir;
	shared shared;
	AudioSource[] source;
	Animator anm;
	Rigidbody body;

//*************************************************************************************************************************************************
	//Get components
	void Start()
	{
		Left_Hips = transform.Find ("Proto/root/pelvis/left hips");
		Right_Hips = transform.Find ("Proto/root/pelvis/right hips");
		Left_Leg  = transform.Find ("Proto/root/pelvis/left hips/left leg");
		Right_Leg = transform.Find ("Proto/root/pelvis/right hips/right leg");
		
		Left_Foot = transform.Find ("Proto/root/pelvis/left hips/left leg/left foot");
		Right_Foot = transform.Find ("Proto/root/pelvis/right hips/right leg/right foot");
		
		Left_Arm0 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/left arm0");
		Right_Arm0 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/right arm0");
		Left_Arm1 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1");
		Right_Arm1 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1");
		
		Left_Hand = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/left arm0/left arm1/left hand");
		Right_Hand = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/right arm0/right arm1/right hand");
		
		Tail0 = transform.Find ("Proto/root/pelvis/tail0");
		Tail1 = transform.Find ("Proto/root/pelvis/tail0/tail1");
		Tail2 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2");
		Tail3 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2/tail3");
		Tail4 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2/tail3/tail4");
		Tail5 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5");
		Tail6 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6");
		Tail7 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7");
		Tail8 = transform.Find ("Proto/root/pelvis/tail0/tail1/tail2/tail3/tail4/tail5/tail6/tail7/tail8");
		Spine0 = transform.Find ("Proto/root/spine0");
		Spine1 = transform.Find ("Proto/root/spine0/spine1");
		Spine2 = transform.Find ("Proto/root/spine0/spine1/spine2");
		Neck0 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/neck0");
		Neck1 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1");
		Neck2 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2");
		Neck3 = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3");
		Head = transform.Find ("Proto/root/spine0/spine1/spine2/spine3/spine4/neck0/neck1/neck2/neck3/head");
		
		source = GetComponents<AudioSource>();
		shared= GetComponent<shared>();
		body=GetComponent<Rigidbody>();
		anm=GetComponent<Animator>();
	}
	
	//*************************************************************************************************************************************************
	//Check collisions
	void OnCollisionStay(Collision col) { shared.ManageCollision(col, MAXPITCH, MAXCROUCH, source, Proto1, Hit_jaw, Hit_head, Hit_tail); }
	
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
				else source[1].PlayOneShot(Smallstep, Random.Range(0.25f, 0.5f));
				shared.lastframe=shared.currframe; break;
			case "Slip": source[1].pitch=Random.Range(2.0f, 2.25f); source[1].PlayOneShot(shared.IsOnWater?Largesplash:Slip, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Die":  source[1].pitch=Random.Range(1.0f, 1.25f); source[1].PlayOneShot(shared.IsOnWater?Medsplash:Smallstep, Random.Range(0.5f, 0.75f));
				shared.lastframe=shared.currframe; shared.IsDead=true; break;
			case "Sniff": source[0].pitch=Random.Range(2.0f, 2.5f); source[0].PlayOneShot(Sniff2, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Chew": source[0].pitch=Random.Range(2.0f, 2.25f); source[0].PlayOneShot(Chew, 0.5f);
				shared.lastframe=shared.currframe; break;
			case "Sleep": source[0].pitch=Random.Range(3.5f, 3.75f); source[0].PlayOneShot(Idleherb, 0.25f);
				shared.lastframe=shared.currframe; break;
			case "Growl": int rnd = Random.Range (0, 4); source[0].pitch=Random.Range(1.0f, 1.25f);
				if(rnd==0)source[0].PlayOneShot(Proto1, 1.0f);
				else if(rnd==1)source[0].PlayOneShot(Proto2, 1.0f);
				else if(rnd==2)source[0].PlayOneShot(Proto3, 1.0f);
				else source[0].PlayOneShot(Proto4, 1.0f);
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
			body.mass=5; body.drag=1; body.angularDrag=1; anm.speed=shared.AnimSpeed/2; anm.Play("Proto|Run");
			body.AddForce(Vector3.up*(shared.posY-(transform.position.y+shared.scale))*64); shared.Health-=0.1f;
		}
		else body.AddForce(-Vector3.up*Mathf.Lerp(dir.y, 128, 1.0f));
		
		//Stopped
		if(NextAnm.IsName("Proto|IdleA") | CurrAnm.IsName("Proto|IdleA") | CurrAnm.IsName("Proto|Die"))
		{
			if(CurrAnm.IsName("Proto|Die")) { reset=true; shared.IsConstrained=true; if(!shared.IsDead) { PlaySound("Growl", 2); PlaySound("Die", 12); } }
		}
		
		//Forward
		else if(CurrAnm.IsName("Proto|Walk") | CurrAnm.IsName("Proto|WalkGrowl"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*15*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Proto|WalkGrowl")) { PlaySound("Growl", 1); PlaySound("Step", 6); PlaySound("Step", 13); }
			else if(CurrAnm.IsName("Proto|Walk")) { PlaySound("Step", 6); PlaySound("Step", 13); }
			else PlaySound("Step", 9);
		}

		//Running
		else if(NextAnm.IsName("Proto|Run") | CurrAnm.IsName("Proto|Run") | CurrAnm.IsName("Proto|RunGrowl") |
		   CurrAnm.IsName("Proto|RunAtk1") | (CurrAnm.IsName("Proto|RunAtk2") && CurrAnm.normalizedTime < 0.5))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.forward*80*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Proto|Run") | NextAnm.IsName("Proto|Run") ) { PlaySound("Step", 3); PlaySound("Step", 6); }
			else if(CurrAnm.IsName("Proto|RunAtk1")) { shared.IsAttacking=true; PlaySound("Growl", 2); PlaySound("Step", 3); PlaySound("Step", 6); }
			else if(CurrAnm.IsName("Proto|RunAtk2")) { shared.IsAttacking=true; PlaySound("Growl", 2); PlaySound("Slip", 6); }
			else { PlaySound("Growl", 2); PlaySound("Step", 3); PlaySound("Step", 6); }
		}

		//Backward
		else if(CurrAnm.IsName("Proto|Walk-") | CurrAnm.IsName("Proto|WalkGrowl-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn"), 0);
			body.AddForce(transform.forward*-15*transform.localScale.x*anm.speed);
			if(CurrAnm.IsName("Proto|Walk-")) { PlaySound("Step", 4); PlaySound("Step", 11); }
			else if(CurrAnm.IsName("Proto|WalkGrowl-")) { PlaySound("Growl", 1); PlaySound("Step", 4); PlaySound("Step", 11); }
			else PlaySound("Step", 9);
		}

		//Strafe/Turn right
		else if(NextAnm.IsName("Proto|Strafe-") | CurrAnm.IsName("Proto|Strafe-"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*8*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Strafe/Turn left
		else if(NextAnm.IsName("Proto|Strafe+") | CurrAnm.IsName("Proto|Strafe+"))
		{
			transform.rotation*= Quaternion.Euler(0, anm.GetFloat("Turn")*2, 0);
			body.AddForce(transform.right*-8*transform.localScale.x*anm.speed);
			PlaySound("Step", 5); PlaySound("Step", 12);
		}

		//Various
		else if(CurrAnm.IsName("Proto|EatA")) PlaySound("Chew", 10);
		else if(CurrAnm.IsName("Proto|EatB")) { PlaySound("Chew", 1); PlaySound("Chew", 4); PlaySound("Chew", 8); PlaySound("Chew", 12); }
		else if(CurrAnm.IsName("Proto|EatC")) reset=true;
		else if(CurrAnm.IsName("Proto|ToSit")) shared.IsConstrained=true;
		else if(CurrAnm.IsName("Proto|SitIdle")) shared.IsConstrained=true;
		else if(CurrAnm.IsName("Proto|Sleep")) { reset=true; shared.IsConstrained=true; PlaySound("Sleep", 2); }
		else if(CurrAnm.IsName("Proto|ToSit-")) shared.IsConstrained=true;
		else if(CurrAnm.IsName("Proto|SitGrowl")) { shared.IsConstrained=true; PlaySound("Growl", 2); }
		else if(CurrAnm.IsName("Proto|IdleB")) { reset=true; shared.IsConstrained=true; PlaySound("Sniff", 2); PlaySound("Sniff", 5); PlaySound("Sniff", 8); }
		else if(CurrAnm.IsName("Proto|IdleC")) { PlaySound("Growl", 2); PlaySound("Step", 5); PlaySound("Step", 6); PlaySound("Sniff", 9); }
		else if(CurrAnm.IsName("Proto|IdleD")) { PlaySound("Growl", 2); PlaySound("Slip", 3); }
		else if(CurrAnm.IsName("Proto|IdleE")) { PlaySound("Sniff", 1); PlaySound("Growl", 4); PlaySound("Step", 9); PlaySound("Step", 11); }
		else if(CurrAnm.IsName("Proto|IdleAtk")) { shared.IsAttacking=true; PlaySound("Growl", 2); PlaySound("Step", 5); PlaySound("Step", 6); } 
		else if(CurrAnm.IsName("Proto|Die-")) { shared.IsConstrained=true; PlaySound("Growl", 3); transform.tag=("Dino"); shared.IsDead=false; }
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
		float spineZ = spineX*spineY/32; float spineAng = -anm.GetFloat("Turn")*5;
		Neck0.transform.rotation*= Quaternion.Euler(spineY, -spineZ, -spineX);
		Neck1.transform.rotation*= Quaternion.Euler(spineY, -spineZ, -spineX);
		Neck2.transform.rotation*= Quaternion.Euler(spineY, -spineZ, -spineX);
		Neck3.transform.rotation*= Quaternion.Euler(spineY, -spineZ, -spineX);
		Head.transform.rotation*= Quaternion.Euler(spineY, -spineZ, -spineX);

		Spine0.transform.rotation*= Quaternion.Euler (0, 0, spineAng);
		Spine1.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
		Spine2.transform.rotation*= Quaternion.Euler(0, 0, spineAng);
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
		shared.QuadIK( Right_Arm0, Right_Arm1, Right_Hand, Left_Arm0, Left_Arm1, Left_Hand, Right_Hips, Right_Leg, Right_Foot, Left_Hips, Left_Leg, Left_Foot);
		//Check for ground layer
		shared.GetGroundAlt(true, crouch);

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