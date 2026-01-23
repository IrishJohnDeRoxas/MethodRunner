namespace IrishJohnDeRoxas.MethodRunner;

internal class CommonInterviewQuestions
{
    public static string ReverseString(string str)
    {
        string reversedStr = "";

        for (int i = str.Length - 1; i >= 0; i--)
        {
            reversedStr += str[i];
        }

        return reversedStr;
    }

    public static bool IsPalindrome(string str)
    {
        var clean = Utils.CleanPalindromeStr(str);

        int left = 0;
        int right = clean.Length - 1;

        while (left < right)
        {
            if (clean[left] != clean[right])
            {
                return false;
            }

            left++;
            right--;
        }

        return true;
    }

    public static IList<int> MoveZeros(IList<int> numbers)
    {

        int left = 0;

        for (int right = 0; right < numbers.Count; right++)
        {
            if (numbers[right] != 0)
            {
                int temp = numbers[left];
                numbers[left] = numbers[right];
                numbers[right] = temp;
                left++;
            }
        }
        return numbers;
    }

    public static long Fibonacci(int n)
    {

        if (n <= 1)
        {
            return n;
        }

        long first = 0;
        long second = 1;

        for (int i = 2; i <= n; i++)
        {
            long next_number = first + second;
            first = second;
            second = next_number;
        }

        return second;
    }
}
