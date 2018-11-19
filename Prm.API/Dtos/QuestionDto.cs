using System;
using System.Collections.Generic;

namespace Prm.API.Dtos
{
    public class QuestionDto
    {
        public string Value { get; set; }
        public IEnumerable< AnswerDto> Answers { get; set; }
    }
}