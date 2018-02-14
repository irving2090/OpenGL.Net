﻿
// Copyright (C) 2017 Luca Piccioni
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#pragma warning disable 649

using System;
using System.Reflection;

using Khronos;

using NUnit.Framework;

namespace OpenGL.Test
{
	[TestFixture, Category("Framework")]
	class KhronosApiTest
	{
		[Test]
		public void KhronosApi_BindAPIFunction()
		{
			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.BindAPIFunction<Gl>(null, null, null, null, null));
			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.BindAPIFunction<Gl>("pt", null, null, null, null));
			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.BindAPIFunction<Gl>("pt", "fn", null, null, null));
		}

		[Test]
		public void KhronosApi_BindAPI()
		{
			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.BindAPI<Gl>(null, null, null, null));
			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.BindAPI<Gl>("pt", null, null, null));
		}

		[RequiredByFeature("GL_VERSION_1_1")]
		[RequiredByFeature("GL_VERSION_4_3")]
		[RequiredByFeature("GL_VERSION_4_3", Profile = "core")]
		[RequiredByFeature("GL_VERSION_ES_CM_1_0", Api = "gles1")]
		[RequiredByFeature("GL_ES_VERSION_3_2", Api = "gles2")]
		[RequiredByFeature("GL_EXT_vertex_array")]
		[RequiredByFeature("GL_KHR_debug")]
		[RequiredByFeature("GL_KHR_debug", Api = "gles2")]
		[RemovedByFeature("GL_VERSION_3_2", Profile = "core")]
		public static IntPtr GetPointer;

		[Test]
		public void KhronosApi_IsCompatibleField()
		{
			FieldInfo fi = typeof(KhronosApiTest).GetField("GetPointer", BindingFlags.Static | BindingFlags.Public);
			Gl.Extensions extensions = new Gl.Extensions();

			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_100, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_110, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_150, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_210, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_300, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_310, extensions));

			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_320, KhronosVersion.ProfileCore), extensions));
			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_400, KhronosVersion.ProfileCore), extensions));
			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_410, KhronosVersion.ProfileCore), extensions));
			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_420, KhronosVersion.ProfileCore), extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_430, KhronosVersion.ProfileCore), extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_440, KhronosVersion.ProfileCore), extensions));

			Assert.IsTrue(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_320, KhronosVersion.ProfileCompatibility), extensions));
			Assert.IsTrue(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_400, KhronosVersion.ProfileCompatibility), extensions));
			Assert.IsTrue(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_410, KhronosVersion.ProfileCompatibility), extensions));
			Assert.IsTrue(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_420, KhronosVersion.ProfileCompatibility), extensions));
			Assert.IsTrue(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_430, KhronosVersion.ProfileCompatibility), extensions));
			Assert.IsTrue(Khronos.KhronosApi.IsCompatibleField(fi, new KhronosVersion(Gl.Version_440, KhronosVersion.ProfileCompatibility), extensions));

			// Not sure how Profile can affect this...
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_100_ES, extensions));
			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_200_ES, extensions));
			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_300_ES, extensions));
			Assert.IsFalse(Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_310_ES, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_320_ES, extensions));

			extensions.EnableExtension("GL_EXT_vertex_array");
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_100, extensions));

			extensions.EnableExtension("GL_KHR_debug");
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_100, extensions));
			Assert.IsTrue (Khronos.KhronosApi.IsCompatibleField(fi, Gl.Version_200_ES, extensions));
		}

		[Test]
		public void KhronosApi_Log()
		{
			Assert.IsFalse(Khronos.KhronosApi.LogEnabled);
			Assert.IsTrue(Khronos.KhronosApi.LogEnabled = true);
			Assert.IsFalse(Khronos.KhronosApi.LogEnabled = false);

			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.RaiseLog(null));
			Assert.Throws<ArgumentNullException>(() => Khronos.KhronosApi.LogComment(null));

			int logcounter = 0;

			Khronos.KhronosApi.Log += (sender, e) => {
				Assert.IsNotNull(e.ToString());
				logcounter++;
			};

			Khronos.KhronosApi.LogEnabled = false;
			Khronos.KhronosApi.LogComment("Any comment?");
			Assert.AreEqual(0, logcounter);

			Khronos.KhronosApi.LogEnabled = true;
			Khronos.KhronosApi.LogComment("Any comment?");
			Assert.AreEqual(1, logcounter);
		}

		internal class TestExtensions : Khronos.KhronosApi.ExtensionsCollection
		{
			public new void Query(KhronosVersion version, string extensionsString)
			{
				base.Query(version, extensionsString);
			}

			public new void Query(KhronosVersion version, string[] extensionsString)
			{
				base.Query(version, extensionsString);
			}

			/// <summary>
			/// Simple extension.
			/// </summary>
			[Khronos.KhronosApi.Extension("GL_EXT_1", Api = "gl")]
			public bool Ext1;

			/// <summary>
			/// Another simple extension.
			/// </summary>
			[Khronos.KhronosApi.Extension("GL_EXT_2", Api = "gles2")]
			public bool Ext2;

			/// <summary>
			/// Test Api attribute as regular expression.
			/// </summary>
			[Khronos.KhronosApi.Extension("GL_EXT_3", Api = "gl|gles2")]
			public bool Ext3;

			/// <summary>
			/// Multiple extension.
			/// </summary>
			[Khronos.KhronosApi.Extension("GL_EXT_1", Api = "gl")]
			[Khronos.KhronosApi.Extension("GL_EXT_2", Api = "gles2")]
			[Khronos.KhronosApi.Extension("GL_EXT_3", Api = "gl|gles2")]
			public bool Ext;

			/// <summary>
			/// Core extension.
			/// </summary>
			[Khronos.KhronosApi.CoreExtension(3, 2)]
			public bool Core1;

			/// <summary>
			/// Another core extension.
			/// </summary>
			[Khronos.KhronosApi.CoreExtension(3, 2, "gles2")]
			public bool Core2;

			/// <summary>
			/// Multiple core extension.
			/// </summary>
			[Khronos.KhronosApi.CoreExtension(3, 2, "gl")]
			[Khronos.KhronosApi.CoreExtension(3, 2, "gles2")]
			public bool Core3;

			/// <summary>
			/// Mixed attributes
			/// </summary>
			[Khronos.KhronosApi.CoreExtension(3, 2, "gl")]
			[Khronos.KhronosApi.Extension("GL_EXT_3", Api = "gl")]
			public bool Mixed;

			/// <summary>
			/// This extension won't be supported.
			/// </summary>
			[Khronos.KhronosApi.Extension("GL_EXT_invalid", Api = "gl|gles2")]
			public int InvalidExt;
		}

		[Test]
		public void KhronosApi_Extensions()
		{
			TestExtensions extensions = new TestExtensions();

			Assert.Throws<ArgumentNullException>(() => extensions.HasExtensions(null));
			Assert.Throws<ArgumentNullException>(() => extensions.Query(Gl.Version_100, (string)null));
			Assert.Throws<ArgumentNullException>(() => extensions.Query(Gl.Version_100, (string[])null));

			Assert.DoesNotThrow(() => extensions.Query(Gl.Version_100, ""));
			Assert.DoesNotThrow(() => extensions.Query(null, ""));
			Assert.DoesNotThrow(() => extensions.Query(null, "   "));

			// Defaults to false
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_1"));
			Assert.IsFalse(extensions.Ext1);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_2"));
			Assert.IsFalse(extensions.Ext2);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_3"));
			Assert.IsFalse(extensions.Ext3);
			Assert.IsFalse(extensions.Ext);
			Assert.IsFalse(extensions.Core1);
			Assert.IsFalse(extensions.Core2);
			Assert.IsFalse(extensions.Core3);

			// Filter by API
			extensions.Query(Gl.Version_100, "GL_EXT_1 GL_EXT_2 GL_EXT_3");
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_1"));
			Assert.IsTrue(extensions.Ext1);
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_2"));					// Extension listed as supported!
			Assert.IsFalse(extensions.Ext2);										// ...but field is filtered by API ('gl' in this case) (*)
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_3"));
			Assert.IsTrue(extensions.Ext3);
			Assert.IsTrue(extensions.Ext);
			Assert.IsFalse(extensions.Core1);
			Assert.IsFalse(extensions.Core2);
			Assert.IsFalse(extensions.Core3);

			// (*) This should happen when the extensions string is coherent with the GL context version

			extensions.Query(Gl.Version_200_ES, "GL_EXT_1 GL_EXT_2 GL_EXT_3");
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_1"));
			Assert.IsFalse(extensions.Ext1);
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_2"));
			Assert.IsTrue(extensions.Ext2);
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_3"));
			Assert.IsTrue(extensions.Ext3);
			Assert.IsTrue(extensions.Ext);
			Assert.IsFalse(extensions.Core1);
			Assert.IsFalse(extensions.Core2);
			Assert.IsFalse(extensions.Core3);

			extensions.Query(Gl.Version_100, "GL_EXT_2");
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_1"));
			Assert.IsFalse(extensions.Ext1);
			Assert.IsTrue(extensions.HasExtensions("GL_EXT_2"));
			Assert.IsFalse(extensions.Ext2);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_3"));
			Assert.IsFalse(extensions.Ext3);
			Assert.IsFalse(extensions.Ext);
			Assert.IsFalse(extensions.Core1);
			Assert.IsFalse(extensions.Core2);
			Assert.IsFalse(extensions.Core3);

			// Filter by version
			extensions.Query(Gl.Version_320, "");
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_1"));
			Assert.IsFalse(extensions.Ext1);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_2"));
			Assert.IsFalse(extensions.Ext2);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_3"));
			Assert.IsFalse(extensions.Ext3);
			Assert.IsFalse(extensions.Ext);
			Assert.IsTrue(extensions.Core1);
			Assert.IsFalse(extensions.Core2);
			Assert.IsTrue(extensions.Core3);

			extensions.Query(Gl.Version_320_ES, "");
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_1"));
			Assert.IsFalse(extensions.Ext1);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_2"));
			Assert.IsFalse(extensions.Ext2);
			Assert.IsFalse(extensions.HasExtensions("GL_EXT_3"));
			Assert.IsFalse(extensions.Ext3);
			Assert.IsFalse(extensions.Ext);
			Assert.IsFalse(extensions.Core1);
			Assert.IsTrue(extensions.Core2);
			Assert.IsTrue(extensions.Core3);

			// Duplicated extension are managed nicely
			Assert.DoesNotThrow(() => extensions.Query(null, "GL_EXT_3 GL_EXT_3 GL_EXT_3"));

			// Mixed attributes
			extensions.Query(Gl.Version_100, "");
			Assert.IsFalse(extensions.Mixed);

			extensions.Query(Gl.Version_320, "");
			Assert.IsTrue(extensions.Mixed);

			extensions.Query(Gl.Version_100, "GL_EXT_3");
			Assert.IsTrue(extensions.Mixed);
		}
	}
}
