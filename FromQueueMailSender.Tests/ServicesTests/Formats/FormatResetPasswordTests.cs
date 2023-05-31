using FromQueueMailSender.Services.Mail;
using FromQueueMailSender.Services.Mail.Formats;
using FromQueueMailSender.UnitTests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromQueueMailSender.UnitTests.ServicesTests.Formats
{
    public class FormatResetPasswordTests
    {
        [Theory]
        [InlineData("Adam Kowalski")]
        [InlineData("Zażółć Nowak")]
        [InlineData("Janina Barbara Żółkiewska")]
        [InlineData("Agnieszka Nowak-Żukowa")]
        public void GetContent_ForGivenName_ShouldContainsName(string name)
        {
            //arrange
            string content;

            //act
            content = ResetPasswordContent.GetContent(name, "");

            //assert
            Assert.Contains(name, content);
        }

        [Theory]
        [InlineData("https://www.deepl.com/translator")]
        [InlineData("https://jtm.com/activate/344@#$2343j4i2342904")]
        [InlineData("http://jtm.com.com/activateaccount/123dfsdfs232i3j2io34")]
        [InlineData("https://google.com")]
        [InlineData("https://333.32344.23efwe/wwefwef34ffd/23fsd234fe34fg345giopj34io5j34io5j34kl5h34k5h4jk534jk5h34jk5h34jk")]
        public void GetContent_ForGivenUrl_ShouldContainsThreeUrls(string url)
        {
            //arrange
            string content;

            //act
            content = ResetPasswordContent.GetContent("", url);
            int urlOccurrences = content.CountOccurrences(url);

            //assert
            Assert.True(urlOccurrences == 3);
        }

        [Fact]
        public void GetContent_Always_ShouldContainsNoReplyPhrase()
        {
            //arrange
            string content;
            string noReplyMessage = "Wiadomość wygenerowana automatycznie prosimy na nią nie odpowiadać.";

            //act
            content = ResetPasswordContent.GetContent(string.Empty, string.Empty);

            //assert
            Assert.Contains(noReplyMessage, content);
        }
    }
}
