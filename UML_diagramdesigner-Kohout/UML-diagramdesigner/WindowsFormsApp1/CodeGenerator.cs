using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using WindowsFormsApp1.ClassDiagram;

namespace WindowsFormsApp1
{
    public class CCodeGenerator
    {


        CodeNamespace mynamespace;
        CodeTypeDeclaration myclass;
        CodeCompileUnit myassembly;
        public void CreateNamespace()
        {
            mynamespace = new CodeNamespace("mynamespace");
        }

        public void CreateClass(Diagram diagram)
        {
            if (diagram.className != null)
            {
                
                myclass = new CodeTypeDeclaration();
                myclass.Name = diagram.className.Name;
                if (diagram.connectedDiagramClassName != null)
                {
                    myclass.Name = diagram.className.Name + " : " + diagram.connectedDiagramClassName.Name;
                }
                if (diagram.className.IsInterface == true)
                {
                    myclass.IsInterface = true;
                }
                if (diagram.className.IsInterface == false)
                {
                    myclass.IsClass = true;
                }
                myclass.Attributes = MemberAttributes.Public;
                mynamespace.Types.Add(myclass);
            }              
        }
        public void CreateProperty(Diagram diagram)
        {
            if (diagram.properties != null && diagram.className != null)
            {
                foreach (var property in diagram.properties)
                {
                    CodeMemberProperty mypropertyfield = new CodeMemberProperty();
                    mypropertyfield.Name = property.Name;
                    if (myclass.IsInterface == true)
                    {

                    }
                    else if (myclass.IsInterface == false)
                    {
                        if (property.AccessModifier == "public")
                        {
                            mypropertyfield.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                        }
                        else if (property.AccessModifier == "private")
                        {
                            mypropertyfield.Attributes = MemberAttributes.Private;
                        }
                    }
                    if (property.DataType == "string")
                    {
                        mypropertyfield.Type = new CodeTypeReference(typeof(string));
                    }
                    else if (property.DataType == "int")
                    {
                        mypropertyfield.Type = new CodeTypeReference(typeof(int));
                    }
                    else if (property.DataType == "float")
                    {
                        mypropertyfield.Type = new CodeTypeReference(typeof(float));
                    }
                    else if (property.DataType == "bool")
                    {
                        mypropertyfield.Type = new CodeTypeReference(typeof(bool));
                    }
                    else if (property.DataType == "char")
                    {
                        mypropertyfield.Type = new CodeTypeReference(typeof(char));
                    }
                    else
                    {
                        mypropertyfield.Type = new CodeTypeReference(property.DataType);
                    }
                    
                    mypropertyfield.HasGet = true;
                    mypropertyfield.HasSet = true;
                    myclass.Members.Add(mypropertyfield);
                }
            }                
        }

        public void CreateMethod(Diagram diagram)
        {
            if (diagram.methods != null)
            {
                foreach (var method in diagram.methods)
                {
                    CodeMemberMethod mymethod = new CodeMemberMethod();
                    mymethod.Name = method.Name;
                    if (myclass.IsInterface == true)
                    {

                    }
                    else if (myclass.IsInterface == false)
                    {
                        if (method.AccessModifier == "public")
                        {
                            mymethod.Attributes = MemberAttributes.Public;
                        }
                        else if (method.AccessModifier == "private")
                        {
                            mymethod.Attributes = MemberAttributes.Private;
                        }
                    }
                    if (method.DataType == "string")
                    {
                        mymethod.ReturnType = new CodeTypeReference(typeof(string));
                    }
                    else if (method.DataType == "int")
                    {
                        mymethod.ReturnType = new CodeTypeReference(typeof(int));
                    }
                    else if (method.DataType == "float")
                    {
                        mymethod.ReturnType = new CodeTypeReference(typeof(float));
                    }
                    else if (method.DataType == "bool")
                    {
                        mymethod.ReturnType = new CodeTypeReference(typeof(bool));
                    }
                    else if (method.DataType == "char")
                    {
                        mymethod.ReturnType = new CodeTypeReference(typeof(char));
                    }
                    else if (method.DataType == "void")
                    {
                        mymethod.ReturnType = new CodeTypeReference();
                    }
                    else 
                    {
                        mymethod.ReturnType = new CodeTypeReference(method.DataType);
                    }
                    if (method.Parameters != null)
                    {
                        foreach (var parameter in method.Parameters)
                        {
                            CodeParameterDeclarationExpression param = new CodeParameterDeclarationExpression(parameter.DataType, parameter.Name);
                            mymethod.Parameters.Add(param);
                        }
                    }                    
                    myclass.Members.Add(mymethod);
                }
            }               
        }

        public void SaveAssembly(Diagram diagram, String path)
        {
            myassembly = new CodeCompileUnit();
            myassembly.Namespaces.Add(mynamespace);
            Microsoft.CSharp.CSharpCodeProvider ccp = new Microsoft.CSharp.CSharpCodeProvider();

            String sourceFile;
            
            if (ccp.FileExtension[0] == '.')
            {
                sourceFile = diagram.className.Name + ccp.FileExtension;
            }
            else
            {
                sourceFile = diagram.className.Name + "." + ccp.FileExtension;
            }
               var tw1 = new IndentedTextWriter(new StreamWriter(Path.Combine(path,sourceFile), false), "    ");
               ccp.GenerateCodeFromCompileUnit(myassembly, tw1, new CodeGeneratorOptions());
               tw1.Close();
          
        }       
    }
    
}
