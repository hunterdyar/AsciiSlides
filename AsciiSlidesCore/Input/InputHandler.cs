﻿using Eto.Forms;

namespace AsciiSlidesCore;

public static class InputHandler
{
	public static void RegisterFormAsSlideController(Form form)
	{
		form.KeyDown += FormOnKeyDown;
		form.Closed += (sender, args) =>
		{
			form.KeyDown -= FormOnKeyDown;
		};
	}
	
	private static void FormOnKeyDown(object? sender, KeyEventArgs e)
	{
		if (Configuration.NextSlide.Contains(e.Key))
		{
			SlidesManager.PresentationState?.NavigateRelative(1);
			e.Handled = true;
		}
		else if (Configuration.PreviousSlide.Contains(e.Key))
		{
			if (e.Control)
			{
				//Cycle Down list of screens.
				// MoveScreens(-1);
			}
			else
			{
				//Go back a slide.
				SlidesManager.PresentationState?.NavigateRelative(-1);
				e.Handled = true;
			}
		}else if(Configuration.ClosePresentation.Contains(e.Key))
		{
			//Close the presentation.
			SlidesManager.PresentationState.ClosePresentation();
			e.Handled = true;
		}
	}
	
}