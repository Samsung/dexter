﻿#region Copyright notice
/**
 * Copyright (c) 2018 Samsung Electronics, Inc.,
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.
 * 
 * * Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion
using DexterCS;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace DexterCRC
{
    public class EventCRC : ICRCLogic
    {
        SuffixNaming suffixNaming;
        PascalCasing pascalCasing;

        public EventCRC()
        {
            suffixNaming = new SuffixNaming();
            pascalCasing = new PascalCasing();
        }
        public void Analyze(AnalysisConfig config, AnalysisResult result, Checker checker, SyntaxNode syntaxRoot)
        {
            var eventRaws = syntaxRoot.DescendantNodes().OfType<EventFieldDeclarationSyntax>();
            foreach (var eventRaw in eventRaws)
            {
                string eventTypeName = eventRaw.Declaration.Type.ToString();
                if (suffixNaming.HasDefect(new NamingSet
                {
                    currentName = eventTypeName,
                    basicWord = DexterCRCUtil.EVENT_TYPE_SUFFIX
                }))
                {
                    PreOccurence preOcc = suffixNaming.MakeDefect(config, checker, eventRaw);
                    result.AddDefectWithPreOccurence(preOcc);
                }

                List<string> variables = GetCamelCasingVariables(eventRaw.Declaration.Variables);
                foreach (string variable in variables)
                {
                    PreOccurence preOcc = pascalCasing.MakeDefect(config, checker, eventRaw);
                    result.AddDefectWithPreOccurence(preOcc);
                }
            }
        }

        private List<string> GetCamelCasingVariables(SeparatedSyntaxList<VariableDeclaratorSyntax> variables)
        {
            List<string> tempVariables = new List<string>();
            foreach (var variable in variables)
            {
                if (pascalCasing.HasDefect(variable.Identifier.ToString()))
                {
                    tempVariables.Add(variable.Identifier.ToString());
                }
            }
            return tempVariables;
        }
    }
}
