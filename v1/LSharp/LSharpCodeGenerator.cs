using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace LSharp
{
	
	public class LSharpCodeGenerator : CodeGenerator
	{	
		protected override string CreateEscapedIdentifier(string value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override string CreateValidIdentifier(string value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateArgumentReferenceExpression(System.CodeDom.CodeArgumentReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateArrayCreateExpression(System.CodeDom.CodeArrayCreateExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateArrayIndexerExpression(System.CodeDom.CodeArrayIndexerExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateAssignStatement(System.CodeDom.CodeAssignStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateAttachEventStatement(System.CodeDom.CodeAttachEventStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateAttributeDeclarationsEnd(System.CodeDom.CodeAttributeDeclarationCollection attributes)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateAttributeDeclarationsStart(System.CodeDom.CodeAttributeDeclarationCollection attributes)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateBaseReferenceExpression(System.CodeDom.CodeBaseReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateCastExpression(System.CodeDom.CodeCastExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateComment(System.CodeDom.CodeComment e)
		{
			this.Output.Write(";;; {0}",e.Text);
		}

		protected override void GenerateConditionStatement(System.CodeDom.CodeConditionStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateConstructor(System.CodeDom.CodeConstructor e, System.CodeDom.CodeTypeDeclaration c)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateDelegateCreateExpression(System.CodeDom.CodeDelegateCreateExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateDelegateInvokeExpression(System.CodeDom.CodeDelegateInvokeExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateEntryPointMethod(System.CodeDom.CodeEntryPointMethod e, System.CodeDom.CodeTypeDeclaration c)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateEvent(System.CodeDom.CodeMemberEvent e, System.CodeDom.CodeTypeDeclaration c)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateEventReferenceExpression(System.CodeDom.CodeEventReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateExpressionStatement(System.CodeDom.CodeExpressionStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateField(System.CodeDom.CodeMemberField e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateFieldReferenceExpression(System.CodeDom.CodeFieldReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateGotoStatement(System.CodeDom.CodeGotoStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateIndexerExpression(System.CodeDom.CodeIndexerExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateIterationStatement(System.CodeDom.CodeIterationStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateLabeledStatement(System.CodeDom.CodeLabeledStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateLinePragmaEnd(System.CodeDom.CodeLinePragma e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateLinePragmaStart(System.CodeDom.CodeLinePragma e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateMethod(System.CodeDom.CodeMemberMethod e, System.CodeDom.CodeTypeDeclaration c)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateMethodInvokeExpression(System.CodeDom.CodeMethodInvokeExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateMethodReferenceExpression(System.CodeDom.CodeMethodReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateMethodReturnStatement(System.CodeDom.CodeMethodReturnStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateNamespaceEnd(System.CodeDom.CodeNamespace e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateNamespaceImport(System.CodeDom.CodeNamespaceImport e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateNamespaceStart(System.CodeDom.CodeNamespace e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateObjectCreateExpression(System.CodeDom.CodeObjectCreateExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateProperty(System.CodeDom.CodeMemberProperty e, System.CodeDom.CodeTypeDeclaration c)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GeneratePropertyReferenceExpression(System.CodeDom.CodePropertyReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GeneratePropertySetValueReferenceExpression(System.CodeDom.CodePropertySetValueReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateRemoveEventStatement(System.CodeDom.CodeRemoveEventStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateSnippetExpression(System.CodeDom.CodeSnippetExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateSnippetMember(System.CodeDom.CodeSnippetTypeMember e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateThisReferenceExpression(System.CodeDom.CodeThisReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateThrowExceptionStatement(System.CodeDom.CodeThrowExceptionStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateTryCatchFinallyStatement(System.CodeDom.CodeTryCatchFinallyStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateTypeConstructor(System.CodeDom.CodeTypeConstructor e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateTypeEnd(System.CodeDom.CodeTypeDeclaration e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateTypeStart(System.CodeDom.CodeTypeDeclaration e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateVariableDeclarationStatement(System.CodeDom.CodeVariableDeclarationStatement e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void GenerateVariableReferenceExpression(System.CodeDom.CodeVariableReferenceExpression e)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override string GetTypeOutput(System.CodeDom.CodeTypeReference value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override bool IsValidIdentifier(string value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override string NullToken
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		protected override void OutputType(System.CodeDom.CodeTypeReference typeRef)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override string QuoteSnippetString(string value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override bool Supports(System.CodeDom.Compiler.GeneratorSupport support)
		{
			throw new Exception("The method or operation is not implemented.");
		}


	}
}
