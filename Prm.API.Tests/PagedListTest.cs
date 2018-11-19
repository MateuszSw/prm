using System;
using Xunit;
using System.Linq;
using Prm.API.Helpers;

namespace prm.api.tests
{
    public class PagedListTest
    {
        string[] testList = {"one", "two", "three", "four", "five", "six", "seven", "eigth", "nine", "ten", "eleven", "twelve"};

        [Fact]
        public void CheckConstantPropertiesTest()
        {
            // arange
            var sourceList = testList.AsQueryable<string>();            

            // act
            var pagedList = PagedList<string>.Create(sourceList, 2, 5);           

            // assert
            Assert.Equal(testList.Length, pagedList.TotalCount);
            Assert.Equal(2, pagedList.CurrentPage);
            Assert.Equal(5, pagedList.SizePage);        }

        [Fact]
        public void CheckTotalTest()
        {
            // arange
            var sourceList = testList.AsQueryable<string>();            

            // act
            var pagedList = PagedList<string>.Create(sourceList, 2, 5);           

            // assert
            Assert.Equal(3, pagedList.Total);
        }
        
        [Fact]
        public void CheckGoodContentTestAsync()
        {
            // arange
            var sourceList = testList.AsQueryable<string>();            

            // act
            var pagedList = PagedList<string>.Create(sourceList, 2, 5);           

            // assert            
            Assert.Equal(5, pagedList.Count);
            for(int i =0;i<5;i++){
                Assert.Equal(testList[i+5], pagedList[i]);
            }
        }
    }
}
