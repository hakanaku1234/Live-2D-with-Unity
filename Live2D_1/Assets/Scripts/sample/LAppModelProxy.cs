/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text ;
using System.Runtime.InteropServices;
using live2d;
using live2d.framework;
using UnityEngine.UI;


[ExecuteInEditMode]
public class LAppModelProxy : MonoBehaviour
{
    public String path="";
    public int sceneNo = 0;

    private LAppModel model;

	private bool	isVisible = false ;

	[SerializeField] Text uiText;

	void Awake()
    {
        if (path == "") return;
        model = new LAppModel(this);

        LAppLive2DManager.Instance.AddModel(this);

        var filename = FileManager.getFilename(path);
        var dir = FileManager.getDirName(path);

        Debug.Log("Load " + dir +"  filename:"+ filename);
		model.LoadFromStreamingAssets(dir, filename);
    }
	

	void OnRenderObject()
	{
		if(!isVisible) return;
		if(model==null)return;

		if(model.GetLive2DModelUnity().getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW)
		{
            model.Draw();
		}
		
		if (LAppDefine.DEBUG_DRAW_HIT_AREA)
		{
			// デバッグ用当たり判定の描画
			model.DrawHitArea();
		}
	}
	

	void Update()
	{
		if(!isVisible) return;
        if (model == null) return;

        model.Update();
        if (model.GetLive2DModelUnity().getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH)
        {
            model.Draw();
        }
	}


    public LAppModel GetModel()
    {
        return model;
    }


	public void SetVisible(bool isVisible)
	{
		this.isVisible = isVisible;
	}


	public bool GetVisible()
	{
		return isVisible;
	}


    public void ResetAudioSource()
    {
        Component[] components = gameObject.GetComponents<AudioSource>();
        for (int i = 0; i < components.Length; i++)
        {
            Destroy(components[i]);
        }
    }

	public void Motion_Change(){
		model.StartMotion("shake", 0,2);
		uiText.text = "因為你溫柔的舉動，\n讓Haruru覺得很開心。\n【好感度+10】";

	}

	public void Angry_Change(){
		model.StartMotion(LAppDefine.MOTION_GROUP_TAP_BODY, 0,3);
		uiText.text = "意圖不良的舉動，\n讓Haruru感到很不舒服。\n【好感度-30】";
		
	}

	public void joke_Change(){
		model.StartMotion("joke", 0,4);
		uiText.text = "即使你說的笑話不好笑，\n但想讓她開心的心意對Haruru似乎起了作用。\n【好感度+8】";
		
	}
}