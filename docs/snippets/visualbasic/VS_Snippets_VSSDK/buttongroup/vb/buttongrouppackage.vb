﻿'-----------------------------------------------------------------------------
' Copyright (c) Microsoft Corporation.  All rights reserved.
'-----------------------------------------------------------------------------
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.ComponentModel.Design
Imports Microsoft.Win32
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.OLE.Interop
Imports Microsoft.VisualStudio.Shell

Namespace Microsoft.Create_HandleCommands
	''' <summary>
	''' This is the class that implements the package exposed by this assembly.
	'''
	''' The minimum requirement for a class to be considered a valid package for Visual Studio
	''' is to implement the IVsPackage interface and register itself with the shell.
	''' This package uses the helper classes defined inside the Managed Package Framework (MPF)
	''' to do it: it derives from the Package class that provides the implementation of the 
	''' IVsPackage interface and uses the registration attributes defined in the framework to 
	''' register itself and its components with the shell.
	''' </summary>
	' This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
	' a package.
	' This attribute is used to register the information needed to show the this package
	' in the Help/About dialog of Visual Studio.
	' This attribute is needed to let the shell know that this package exposes some menus.
	<PackageRegistration(UseManagedResourcesOnly := True), InstalledProductRegistration("#110", "#112", "1.0", IconResourceID := 400), ProvideMenuResource("Menus.ctmenu", 1), Guid(GuidList.guidCreate_HandleCommandsPkgString)> _
	Public NotInheritable Class Create_HandleCommandsPackage
		Inherits Package

		''' <summary>
		''' Default constructor of the package.
		''' Inside this method you can place any initialization code that does not require 
		''' any Visual Studio service because at this point the package object is created but 
		''' not sited yet inside Visual Studio environment. The place to do all the other 
		''' initialization is the Initialize method.
		''' </summary>
		Public Sub New()
			Trace.WriteLine(String.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", Me.ToString()))
		End Sub



		'///////////////////////////////////////////////////////////////////////////
		' Overridden Package Implementation
		#Region "Package Members"

		''' <summary>
		''' Initialization of the package; this method is called right after the package is sited, so this is the place
		''' where you can put all the initilaization code that rely on services provided by VisualStudio.
		''' </summary>
		Protected Overrides Sub Initialize()
			Trace.WriteLine (String.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", Me.ToString()))
			MyBase.Initialize()

			' Add our command handlers for menu (commands must exist in the .vsct file)
            ' <Snippet5>
            Dim mcs = TryCast(GetService(GetType(IMenuCommandService)), OleMenuCommandService)
            ' </Snippet5>
			If Nothing IsNot mcs Then
				' Create the command for the menu item.
                '<Snippet6>
				Dim menuCommandID As New CommandID(GuidList.guidButtonGroupCmdSet, CInt(Fix(PkgCmdIDList.cmdidMyCommand)))
                ' </Snippet6>

                ' <Snippet10>
				' Create the command for the menu item.
				Dim menuCommandID1 As New CommandID(GuidList.guidMenuTextCmdSet, CInt(Fix(PkgCmdIDList.cmdidMyCommand)))
				Dim menuItem1 As New OleMenuCommand(AddressOf MenuItemCallback, menuCommandID1)
				AddHandler menuItem1.BeforeQueryStatus, AddressOf OnBeforeQueryStatus
				mcs.AddCommand(menuItem1)
                ' </Snippet10>

                ' <Snippet7>
				Dim menuItem As New MenuCommand(AddressOf MenuItemCallback, menuCommandID1)
                ' </Snippet7>

                '<Snippet9>
				mcs.AddCommand(menuItem)
                '</Snippet9>
			End If
		End Sub
		#End Region

        ' <Snippet11>
		Private Sub OnBeforeQueryStatus(ByVal sender As Object, ByVal e As EventArgs)
			Dim myCommand = TryCast(sender, OleMenuCommand)
			If Nothing IsNot myCommand Then
				myCommand.Text = "New Text"
			End If
		End Sub
        ' </Snippet11>

		''' <summary>
		''' This function is the callback used to execute a command when the a menu item is clicked.
		''' See the Initialize method to see how the menu item is associated to this function using
		''' the OleMenuCommandService service and the MenuCommand class.
		''' </summary>
		Private Sub MenuItemCallback(ByVal sender As Object, ByVal e As EventArgs)
			' Show a Message Box to prove we were here
			Dim uiShell As IVsUIShell = CType(GetService(GetType(SVsUIShell)), IVsUIShell)
			Dim clsid As Guid = Guid.Empty
			Dim result As Integer
			Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(uiShell.ShowMessageBox(0, clsid, "Create_HandleCommands", String.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", Me.ToString()), String.Empty, 0, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_INFO, 0, result)) ' false
		End Sub

	End Class
End Namespace
