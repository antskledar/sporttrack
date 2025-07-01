using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace SportTrack.Tests.Domain;

public class EmailDomainTests
{
    [Theory]
    [InlineData("pero@algebra.hr", true)]
    [InlineData("ivan@gmail.com", false)]
    public void EmailDomainValidator(string email, bool expected)
    {
        bool result = email.EndsWith("@algebra.hr",
                       StringComparison.OrdinalIgnoreCase);

        Assert.Equal(expected, result);
    }
}
