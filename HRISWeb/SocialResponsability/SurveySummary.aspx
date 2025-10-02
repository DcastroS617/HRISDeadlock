<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SurveySummary.aspx.cs" Inherits="HRISWeb.SocialResponsability.SurveySummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cntHeader" runat="server">
    <title><%=Convert.ToString(GetLocalResourceObject("lblScreenTitle")) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">

    <script src="lib/html2pdf.bundle.js"></script>
    <script>

        var imgData = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAK8AAAB1CAYAAAA4N7E8AAAjuElEQVR42u2dB1xT1xfHI0sEEZFNEkCWqFj33tuqdVar1r2qogKBqtW2rtbR1tGqtVpHtY5/tWqtE4Lb1q3sqey99wrh/O993BdeQlgyktB3Pp/zIWQR8r7vvN8599x7ORzWWGONNdZYY4011lhjjTXWWGONNdZYY4011lhjjTXWWGONNdZYa3hz9TQd4S7k/uDuxd/r5m0+jv1GWFN6W3iPoy0Q8m54eFsB0xHEoQJP7vduXuZjlgs5+or4bDMucFqtuqFr5n7P0NHlrmlf19sWo1xv8aYJhPzFAi+uq8CLv8lDaPkd+nnQXcg/6S60/MPD2/I6+umNfj5y97Z8LfC2DELPeYv+p2h3b6skdF+ah9AqS+BlmYt+L5RyoWUBfgz9TMXPxa8RCC2D0O8v0e176Od19LfPunvzD34u5G9Hf9fZ9S5vmounad/Ff+lYsDQ1sSFwz9HA7ng8BG6E74Yfn02SBtnbsgwduEjkQoE3/5jAk/8NOviubp68JW63eXPcvHhTBd7cie6evA9p97hj+RE6yFM8PPmzXL2489y9eUvR81aj+9zR39zs5oWivNDyMILjd/T+19D99xBwr9Dtd+j+dPSzRPaEamj/3Nsa1t+xl/g6b7t6vR/63MXIA9D/cgWBvUPgzZuLTv5eE49wdFjSGiGyIUgK6S/fJ+kG0JaUFw43334Ph1/Ohm0P+0FjgyTPN97rBFse9KROqu//HQP7n30EP7/8BI6/WQSnfVfCOX83uBKyhfqc9yKPwL+xZ+B14lXwT/GEt+lPICbbD5JywyAtPwayi1KgoCQHikUFUBvDz8stToeMgnhIyA2FyKzXEJhyB14kXIL7Ub/C9bBdcD5AgD7LEjjwfCrsRJ9x073OVUFdioB+gU7aXS63TSeuudmOx9JXT0NRc1xFFGoPhaK8Kg9mSWkRRGa+gmdxF+DW2z3wR+A6OOGzFI68+pQC6tCLj+HAi2lS/vPLmZT/8moOHH01Dx3oxfC732q4GLQRroXuAO+Ig/A45jS8SbwGwan3IDrLF9IRaBgyVbXcojQISX0IwncH4AQCe+/TD+Hr+10rA+3ND/Tw5gpW3zbpig5FC5bGukoGL/55+svc+XgwsNZ4lp4fC3fR1eHA82mw9UEv2fwiX+DFW+cqNLHhAAtyrczDy/IV/QWe83dhCWtCyy5MgsvBXyFJ1gd9/9YVMKMEcc1t88FY0rGEVmNIi8XQX9qj6BMsUQoysVgEfwZtgo13OzEhFgs8zT9beJKjzZIqY8tfcjRRdi+uSNausxQpgSXmhKBo3FdKVrh58dazxDKjrqfFaOYXlJAbwpKjLJG4TAw5KPE7hBLhiooFP8fV0/xTllyqvss9XwGvNZSKS1hqlMzKysqoRO/wy1kSKYE18YorZtbNnc8W1bm70CqYhhdfplhT7kgcmfmSqgiRQaMidy/ekeYOrRpydRnH96mhZC2Ohven51NYQlTAiksLUGL9G3PkM3C1l5FFc4MXA6qBXAt5S+TaxFsS10LJWh79JeABB9ZUx5LzwmHf0wkEYkuRq5C7vLmAi6OrJoFVF3kb5G2R6xNv038+h4v7FWh470cdZYlQMROhHOV62I6KioSQe3jGBSpYqXTEpcHF0LZDbozcnLgZ9uk/tBvLrDS8TLjM0qCiCZ1f0k1Yf6e8oUggtPR0vdK2rSrCS0sFDK4eciMCrCVyG+R2xG0/PWzkzoTXL/kWS4IKW0puBGx50IMeavZdftvIXBWjrhaRCgYEXFxSsUfeCXkX5B/gn8vOmZ1nwhuUerdRooI4v+D9vVg1SndlIhFkCR/C27mrIbDHuEpekpreJJ+jSJQPOx8PoxO5d6qUyNFatyWRCyYk4mJwOyPvjrwn8t7YV//F9WHCG5r2sOHLOyUieGPcpV7uZz8AwuesgtSzl6EwPJICRdnAfbfApdr/oSQptck+T6lYBD88GSsBeNUFXTNVkgytSHKGo257EnG7Ie+DvB/y/sgHuN2yTGbCG57+r1LCK+u+DoMh89YdKC0oUAp4c/99WeNnbkp4y694Yvj+39Hlx9aLH7L4qpGeKsCLEzXcsW+InEuirhOJuBjagcgHIR/idoufqorwSpzbA3L+eQ5lYrFiteaJ80oHL/Xdowj8zaMBRANb/jPjAnVVVnq925okanzkDkTj9sLRlgYX+VC325bpKg0v8fCZKxQKb8yGHUoJL7bCkhxJl5rAi39ZVeA1Jnq3A/KuROfiqDuYwDsMyYZMlYXXpAsUvVUHX5vO1O8B3cZQf0sh8Hpsoz5D3CYzpYMXW2yWL9W3gmv6rkLeZmVO1rRIiQzDa4XckaF3BxB4h2J4Bbcts1QhYavKgwfZlQNs26k8ses4VCEAY3ijnHmQdVNXKeHF9jTuf6Spx6p47XXznqpQacAlso4MeAcy4UWyIZsJb3AjlMoaWzaEjLGFwhAN8LUrBzig54dNDkbG5XVQ8EoLXQ2clBZenMBdDPqCrkDEc5RM/9Lw0qNqZoxKAy6R9SXwDpHA68nPYcLrn+ypkpo3dBwCOEhTEoHfzl7ZpGAUha0BH36nBtO8pfkFUBQdC0VRsVBaUNhgn7NAlAPbH/WjAb6mbPBqkMirT+C1kYGXTtaGIR/udpufy4T3TeLfCoHXh9cT4rfsgdj131bySOeNENBnQs0Aj7eRAjjzRs1XkdLcPMj55wWkX7oCxRH7IengSUj931XIfRNADa7UWjZs3FqvhE2UlQ2Je49C6KSF4Oc4pHKdG92HH0vYcwSKk1LqdTyiMl9VNPLcthilbPBqM2q8tmRwogep7w4iUXc4doFnRUcZ9ufxFxUCr5/9wBpH6fL9QyBq9Zfo0vxB1QB/aANFwRoUwFW9Z5koGBL374PgETPgjVm38teadYG8B3rS74dOqOQjcyHz75Mgys2rVcJWV3gxiFFrv4I3pl1rfZXBJzo+oYti49/7mPwdup0GOEPZ4GUOUNgw4K0ceWVkgzDigFLCy7SsO4/R8wdV+V5hkxHA4RrgY9OZGq6VlIzeRsG7pR6QctAYolZyK70u/2XLysDFqIF/lw5UPTlu+34qQjYUvKlnLyGp0eu9pZKPVR9I+e3Cex2T/JJMycIoAi/eQWWE16IK2TCUjr5INkjB+78Ad6WHl9JuYRHgZzew6gg80RZKotXAr3MfEIvFkHoGgYIApB437QLFbzWpUhvzNcXhMvCaOUFZJkcqCfPvNAxynjzDl4L3hlcsKoWYL3Y2mN6PFmyuk8ShLSBFKJEPy4UG+soAryaB14DAa8voaejDSNiGUaUyT+lS2Y/PJ6sEvFTkQkDWlMSVxKtBtMeGSo9lnNeHyGV8qfvEGRyp32O/4ELCN+aVXpt+Th8K/M9KAVNbePFroty2NHjCGrFi/Xv0P5Sg5K0vmdTJu8NR8Go9svDKkw0DpGSDzCDF9kaYw9ZY8GILHT+3+jLaSHsQJXPAt710JSB8si0UhaHoa1oeVf06dAbIqYiyPhZOIEZR19emo9TrEr8zgfSz+ui2EyT/fEoCsDS8ThDU3x5yH+hAQC97KXgTfzrWaBWXVCQh6hqBk3LfSY79untcnrLAy5QN8jQvkQ3Sw8N4nTJxWanKwJty/HytqhClqRyUsTtKAVaGIm38ZhMJzBjewD7lkEcstSqXDGYVkiHahQeF/tK13MxbdxnwOkFAtw6Qc0cX8v5pVal0lucTWJEg1uRmXVHS2R+dPP1r/xosfVLS6lz73fqgL93/+1CR0bcq2SBb560YpLgp3ZiDPTE3TGXgFRcWUdD4WFZfYw2dYENF0sBeDhXgXTCggPW1d4TIpTzqdvYNXZSgOVK3y9I51Hvj58astwBxGjoBOkpH4jfm3UGUkwvx2zdD5jU9KAopTxTljgaO+LjmJMyyNyTs+QV9ZxU9zLjpKOmX0+Br3bfG1wf0nVDnJqWMggTJsRd4WvCVCV7ZhI1uzKE0r+t1foIsvL7JN1QGXmw+7XtAaaI65D/TBv/OjlX+jdgNXArKt59YUb+n/GRI/Z7/rBVkXi4HOf9JKyhFMgPfFieXa+Cs63rU72ET2suJkOgkuDYLREnOEDzAoX4DLRPn1/i/RiwW1Bx9E5PrHH033HEgAxeKm0ZfXamsO6O3QQLvZ+fN78nC+7CB1yprbHj9u4xA2rUzBPZ1gLx/WyEtqwFvP5YDGoqieY9bQVk2B7Ju60HCLhMKSirKZpGf2RzJfSnHTaEkSo26L/OyviQK055y2BCKIzQgfKoNvFvkWr8ejREza6VXy0pL4d28tdW+V9JPdT9+/km3JYuHr7lpZqzoQQp9RiO6bLVhMA3vzL3Gh2Th/d3PWWXgxZfIN6aMQQukRwO6doC0k22hJFIdkg8YSmlULC/EaS3KI2t6Bag1uX/H8oge2KcDZF7RA1F8C4hcxgMfXqeG6c8Y9ymET1tSo4dNXVw+uFJDj3OZqO55y8a7juWzj725CxQNL+5tMGU05nSXk7ANG7KsjYssvN886q8y8OLhXfnv6QT+To4oChlRkBa8aolANoCQUTYoYraTAyjSvPlLkH+CbmtKPSZGyV7+U20oiSiPwunn2oCfQ8dG79eoj1c1mFKdHX+9iI6+SYpoWq+uq0y2zkvBa9KFM6Xy8vPWkFecrhLwxu86WMN7O5XLiYetGDC2YMBpDFDyWHrQQVwMUHxabgSOXm1RbedYldOW7DtB6Dg79HoeJO83goTtxo0Kb1F0XJ2/y+zCZKrahBlw9eJ2UyS8eozJl8xm9AFM2YB8hMCrYi8K2vF+DsoOLx6pCug+tpYH1AniNphLtK3ERQFV/4Hic1LPfTffqpLupUfsAnp0QNq3PcQIzCH5x3aQ8Yc+5NzRgUI/LRS1UUJ4tTUFbbQLFyWM1hDYu0OjwlsY+u69jtXWh73phUu2K7IZnTmTgp4G1IcxSEE354x0vcFPkYUXN24oO7z01JvgwQ4IiPYQOr49uu0IAd0cqJqun+0HyJEG7u4AISNtIXIJFwp9tCqALPSoSVCj5+lIAVwcrgkFz1tB/uPWCEpdKArWhMIgLcgRtoa0E+0gfos50sKWEDbBBgIR0Mw6cVM6nmH9PnYuwE3SsNPUq+7IwlvVHDY68o7A8K69xo+WhRfPOlVmeJOPnpW89t2n1pDysyGkn9GHrKttIPeeDhS8bIU0qmb1iVjxLzX/oRxLqdeURKtD5gU9iF1vBiGj7cDHqrNSat7ihKT3OlZp+dGSrQXw1luKWuZJlyzxxCWr43SRmYA5lIZ36Rkz78r7krWntm9SNnhxY3bsxt21P5DmuAzlAHEbzSD7mh6UpjD0bnEtSko59pLnZ/2t1yiwBg2aUr9FWeT4+zTqVEiHXgpbhZ059Z05ysac+j6YAe+oEc766+XtGfYg6phC4cUHoLSoGErSMiA/MBRSTvyvXi2EoePaQ/E7jQp4i3bUVL1Hz2sjFXkLfFtC+Awr8OE1LMR5r/xAWUyygLW35XNF9zfINuf0Z0ReLB1G6hhxJuLN7WTh/fX1/CaDt/HcCYKH2FG9BpWlA9K/4oxqwnyI1CBGaZI6itxq1G1RjDrEbTFFUdMO/OzqXzbztR1Qr+byhrSH0SclDKy4bGqiyIEKefPYpOBFPtrlWuVh4i/uOlLLZ6ocvCZOVMIWsZQPYiQTSuNboERLA4oCNaHIr6VkkILy3H4I0ozKEbc0TAp0DKyvNRmQsPgAgkfao+TMBPL+xfVfdch/rU1JE1x18LXpJLcq4WPRvfq+BosekPfGX+7sZ7FIBPkBIVQfcCxKVKv0L3bUeyksvNbZ50T3Crx44xVdLmNOf+/LGB4eTsO78k+un3zp8KtqwIuAxW2PUWv4CFQtJA/UIX67Bbyxln5eYH8H+clb/mqAkofIbwDkjZL7nAIE6BvzzlVq64iF7SHHqw06WdSg+K2GpN2S9sgV62v9/0QJtkD6n9ch/dINBOwOdEL0q11b5O9/Nkj03UZKZu5C/nZFl8v4jFpvH5mEDU/AGz1+k+FuefAefDFdueFF0CR8a06NouHZEamnquncsuhCdYrJ9jDU5DEeXBAlqFG38x7oyq/11sJxP29Az3GNehLj3oeGsEMvZtBLRHkpQ8XBnlHrZUZeCl4dQ84Ud2HFXmySBmVvGygQ5SkVvL72nSHl13YgilOD/KdaEDbZlupZiP1qN9XrkHz0jNzIXJrYgpotETatPbXKTm3hjXblU6/37+YIRUGa1Pw4f0fpnga/zsNrBS8uYeE2yoY/ibtDaU5ug+ney8Ff07sOFTV1j29VSVsXxpJPwxiRdwzyD52v8ELlRd8L9dynoqHhTTtpAKFjbVGSQxIl066Q5XlfUh4qTk2Xhr2DI9XeGOWMILQoj5opB8tbIEVR6tRQLa1rRXHqkvsx5DhCS81jM3OCd3OtqBMnaiWvfJBk+Ax4N9+lxs+N12LAlvviTZ1mC9cG3PyA0AZN2l4lXKnYJuA2z04ZGnQ6kXIZPcqG4R2NfCzycfN+NrskD97ND3oorWyIWf9tpUWbMcQppy9Sl/fIpZZQEqsGgT2le23zn7eipIB/pw4Q625e3oh+S5ea90YNRkSqU68P/9iaAhgvKyW9joIjZFxsA8HD7CHPNxDp3epbIsMmLpCqvWLY8EyJ+lcp+lNlRGzB4amwY9+/4LzeCwoK65doZxelVOw05MmfosikzYgME9NJW38ZeMchH2/dT3OBPHjr2+Pb0PDi/l28OEhhNc0n4uJiSD83CjLOt6k0RIuBw0AGkeZxGt6QUXZUkiVKouGlm9jNoTBAS+5QL54OTzWJVwMvrk0XRsZU+owl6Rkowfzyvb+HyBUboIRM+ykuKYU9Pz+D5YKbcPFqMFy5Uf9dTCVT4735XylK9+rI0b39SMI2kgHvBOQTnS/Jlw71aZPESUT02q/e22M3fAtJh05Cxs07UBgeUetpLuLiPPDtOLTSQc+9rwMxbhXrNmB4S6LUJYlY+GQbSjYwX5N9szXEbZSeSRyxxF3yWZIOnYLIZZ9LeZTL15By6iKIMrOqGQcpgzyfAIhc9UXtoLXoAZGrN0LeG+mGohIE79bvH8PGHffh7uMoBG/9ZcQOyUaF/OOKGmmjV88xYwxW9CGjbMOZkRfDO+M7o9NVRV9fFdxsBc9xC+j1IWOqjQ01QVJqehCCN+OcfkVk79gRSpPVKrcZomQtaED5bGC8Yo24gbcWEGXlQOYNb0jYcYCCP3zGcmrd4cjP1kHCzoOQefMulFRzIjx+FgfHz/rA4ZOvICe3qN6fB1eaystlTVtxqKreS/f20j0OwxiaF0fejwys1OcIvCyL5MGLa3+qaDg6Riz/nIqsePZvUD+HSvAm7zWWs4aDk8zwsh3kPmoNiT/+it6zDJq7nfJdQcMbrgh4mSUzZodZd6J7aelAy4aPkE9edNL8flXR903itbpHFJEYvB9EKtzjLo6GkCO8ShE15YhJpf7apH2mECBz3ymLMfDPtUuQmX5PYf+D8H7NzxE30Il1KfhLGt4CRUwLYm6uwmzSoeu9Q0ipDEfeicgnIZ/Sa6buuqrg3XDXsc4dS9k5RdDCZLeCfRekROqAkcPXMNRkDRwxHgX3jHuWtw+GtawUZf06dIT8Z7pw07gffG88HmxNN0reK+iNEXQd4qIE/5N8x8GiIczz3V7JcV+ogA0JZaWDKWNCZm+ie0eQyCuBF/nUFX9YBFYF8NWQbXUCWBngvXqlE0yctajS/R0QlEVp6rDdfCocNB4DPxmPgy/Np8N8+wVQkq5GQV/pNX094Ie9Q5o9vI+iTzBqvcZ2ipQOdNWBR6RDD6J7RzA0L4Z3MvJpw1cb7K4KXuy4Dqgq8OpYboeMWG1QN9tZ6TFNi53w6G57ua8rTm8B2rxvlBbSxob3ZfzFCnibuDFdXvSlu8xsSZ8DXTIbIxN5P0Y+0/kyN6oqeLc86F3rZaEUC+8uyE7QBBPHr+Q+3n/sKtizb5Dcx3yemoOB3df/WXjxKvmSlXS8uCMVDS/dqGNNpEMvontpeKmoi3wG9iHL9HdVF31rO0lTkfDqWm2D1Cid93qtvu12WOMx7T8Lb1j644qZNULedEXBqyazDBSXUXUYRCoO4wm80+nIi3zOZ+ctgqsDGM95qsly84rBafBxBfkx6DLk2Hu9tuuwoyDK6kK9h+I+f929oeCNyHxeMUTsxZ2nyGWgmOs5MBO3vqTeO5aUyujIOwv57M5jdDZVB++2h32hpLQQWGt+xoTXzctikSLhlZe4dSCJ2yAyWIGlw1QSdTG8c5DP++RHE6/qAD7jt4Y90s0cXvc7/BWKXrtXXvTtQqLvcCId6ITtEwLvXMP2ms7uXnxRdQD/E3OaPdrNzMLTn0iOr6sXb6mi4ZXdVJueYdGT1HzHEOkwncD7KfL5yBf0X6S3tzp4P/e2gZDU++wRb0YWmOJdUW0QcucrGl55a5nRTer9SM13PEM6zCHwLlRX5yxZeML0dXUA482ZY7J82aPeTIzZkC7w4s7gKNhk+x0MZLQvHX0nkaRtNoEXi/XFrY3V1rjdlt+0U1H/7QXJeW/ZI98M7J+YU5Lj6iLkj+UogcnWfeluMydG9J1ApANO2nCJZCHyJciX2w3U3ooXIK4OYLw9aG1KaKwpt3lHHKzQvLe4/ZUFXjWO9E6ZdKM6XfcdS2q+WDrMxZoX+VL0Ipxxrpy8xfBqTQBve9gH0gpYgFXZroZulRzPVULzjhwlMXntkpak17c3qTzQ0XcOgRdH3s8wvMhXLzltHlITwF/f7wopee9YClTUzvivlRzLpVdNTJUJXhpgLTnJ2wDSKjmJRN95RPcuJ/A6I1/rfImXWB282NffsYOYbDaJU0U78upTevq7mAMUKxxlA1idIR8siHzoRpK3cWTEbRYtHejIi+FFLljzNz+jJoCxP4v7g6VBxeyHJ6PpBffiOUposvKBrv3SU4WGEfkwg1QdsHRYwYDXTUuXs8HlBj+/NgDjVVjqs/Qma01rWx70JDVeqyccJTV51QcrRt8DLR9mkarDZ0Q2rEHuityjLVdtm8vN2gG86a4TFJbksGQoueG92ej9KdyFln8oM7xVbcRC69/RZNh4NiP6YoDdMLzINxjbaexce42XUxuA13vbwb3IX1hClNiis3wq+hq8+Xs4Smyy5TO6ad2ONK0PYuhfXDpbTKQDjrzuyPEK2l8Y2Wrsdv6Tl1YbgMu3DhgDGQVxLClKaPejjjLaIRXblFNXgJlrnNkxErgPif6dR6LvGhJ9KXiRb9K30Ni95JR5VG0B3nDHHq6EbGbbKpXMzvqvYTTlWAzkqIDJa1w3ZwxgDCUJ3ExSfVhBEjcPAvAm5Ju19Tg7Z+4xeV1bgOm+4EfRp1hqlMT2PZ0gOTaK2s61vhGYua8FPWlzKGne+YQkcCvoxI2GF/kW9A7fjFij74nEflldIN71zwh4mXCZpUeBFp72mKrPk2QtnqNiJts+acCoAXcn896YAK8iAOPo+xUBGK+ovdNmgPbJVZd4mXUBGDteJ+tJ7FkkJ4pYmhrI8ksyIT4nCILT7sOLhD/hYdRxuP12L5JtW+DPoC/gfIAAjr1eTK3JLNG7QsszHBU0WQnRliEhujGSuBlEQqwkAG8gAG9F/i3y3S111fbN2m8cVFeA6X0xPN/uozrV2Bpx7Sy7MIWaBfE8/iJcDd0OP7/8hBqqf5/v393TYjRHRU0WYOYGLbiMNpC0UE4nVYjPGAB/TaLvDgww8j1OE3QuOV/i5rzPl0hvMRCQ4gk5Raksocjw0gOZhQkI1JfwNO48/O63GkHaHd73+5V1Ny/uIY6Km2wVoo3M9Pk+pI1yMpl5sZwkcVhCfEkiMAb4B+R7ke+fvssowM3LsqQ+Xywedw9KvQuZBQnoIIqbPahYPmUUxkNstj+SU+fg5Jtl1FWpoUCVirZCy2TBLe4X5LhzmhPA9EicEadi9Z2eDB08kzTxYB0sICW0LST6fk8A/rGlHufIopPmie6elffBqKvjYUys3ZJy30JecQaIxaUqCSiWRbhkiP+HjIJYCM94AjfDv4e9T8c3BJBit9uWhS43+Jlrr/GTVl60CFl0wlQ4+4Dxb7P3G303+4DphnmHTZdN292mH6n1q3GaeC+KphpKZpbS6BV46KlEo8g0IjoKuyBfR2TEt7SEQP4T8kMmdupnlp/nprl71R9iZifbb74rwCfpJpWkFIryEBTFCo/Q+DIvEhdDcWkB+ky51GeLyfKDR9En4ZTvSmr7hAaJnN6WZQJPy2K3W/zcpb+bvxjp0nZ/G3PqeHxEerVHEMnXm1w9bclxbEuCk3pzA5cZgWV1ML27vKNMFP6YVCNWkpG4TURG7CQA70d+EPlhA5762WVnLdIa4zLI9J+eT6F2tH+V+Bck5oY2aDUD9wPkFadDXHYA+CNtfi/qKFwMXA+HX82WNLk0piNYs6ZuN/q9tTF19RtPGqsGkSar7mS2TEcy8NSeVJBMCbStCbgazTHqViUj6P3e2jKSuc6MrjRaSiwk/RACooW3M6TEjzgKIz+qa6h2ZvI2w5DGPtDSM5+tYd0dGypi4xG/DXcdkJ7sINfxY5Sj5+Hnr7tjSxpYrKEpPzP21Vd5cRM3Gx42slGf3bINVfkZTL73rmReoh1psuISSHGQaUeOlR7pJNQmx7DZQ1tdNUKbU7F5C73TPN2ZhmdmTJTRwzgSb2RE4r1EShxBfly7Deecw1BtLxSNM5saCmX1FX9YhE3YZHjAZpD2IgMu9X0OJFc6J5J7WJPv3oQch7YkwdYlV8mWBFRNAqs6A9j/DLRVdaVpkUENuiLBIxDTC5uMYPQHz2OMzm0gSR0tJw4g/wVDjPxkOyv1yz2nt3687Kx5xn8FVFdPfuGCE6aPRrjo7+o4VnspA9ZeBFZ7RlQ1JvlHG3IVpEHVJMdFnSH3WvyXYa2tlNBh6GEmxL2JJh5HErs5pM1yDRlipiXFdyQaHyDR+AQGuS1P/c/OY1rdnfqN0VvX6/ziZgHqLX7+ktMWLyZtMTzZb67eRtt+2gtJ4ktH1i6MyGpBIqsBI6oyL/3qLKQNDzEtJ9qTpKE76RUeSSTFxyQaL2WATMuKXUQb76O1MfJj6K/8Zt5J66/uk1s/GL/RMGjxbxZpggasWjS0r/2bn770tIXvzB+Mr48WGBzsM7v1Jn4PzUWkWw9flfozImsHmcjargadyoLawFKCCTE9wNGOJA98Eo2dSHQZSECeQPqGZxN9vIKU2zxIzRjD/I1syY3IjGPqWpzT3C5af3Wd1Pru8FVtn03ZZhwy97BZ7PLz3EzXG40TqV1u8AudL3NTl523iFjwq5nPrB9N703aavTXaLd2x/rO1fvOfnArD20DqlQ1hQHqIDLAg5OrTuS7sCZD8HRk1WfoVWZkZWFVEMTaMtHYnJTZ7EmC153Ui4dwKpZdnU5gxpfTZSTho4HeQKSGpBmISA56RG8fKcnhisZPLVurHTa21fytfW+tM53G6FzsOU3vr75z21wbuLjt9WEr9G8PW2VwE/vw1QbXBi9t+9eQ5W0v9V+g/0ffT9uc7jVd73i3j1ofchjW6ger3lrbDa01Nqi1pMqA+Goxn0ig6WS0cTwZOh/KiKg0qPbkCsQlFRojRgVAh3xHTM2qxoKqHBDTINN1YrrMZkQOJNbHNuSyiaNyDwIzvekhDfRUkvjR66ktIhDRU5TWkGFqnBC6kRKdgFQ5aKcfW0vcmfhnZIBlMXnfeaR3YyaRN5OJ1BlLtOlQRiTFV5EPGJd+G3L5p2uqRtUkV2wFQAUgbiFTZmNG5Dbk4BoTmPnkcmpPBkG6kOjcm0A9iCO9Fe2HJOrRe2xMJpfrKUSOTCVO3z+JnAwTGNFyNDlRhpGTZhCplvQlJ1M3AicdRel6qiW5ktCQGpIrjF4V5SoW1GYCMzMia5MDrUsOvAEBwYQAzWVAbUMy8Q4Eps4k8nUlkHUnwDG9O/FuxD9gwNiRvJcDGTalIyedPNER1IR8pnYy9VT6sk8nVbRW1WBB/e/IC7o+yYzMdHTWJbDoE3DakUhnRKAyJZCbEdjkuTlxMwaMxgRIQ3LCtGWAqUcu8zpyAJW95LMVANbkygwaag0G2C0ZMNFRW4cBuq7MbV3G4zrk+a0YiZIWA0pNmcipxgLKWn1BbiEnEVSX4xpVuOzz1KpxeX+TNdaaBO66OGusscYaa6yxxhprrLHGGmusNX/7PwCSKEprltykAAAAAElFTkSuQmCC';
    </script>

    <style>
        .pagebreak-before {
            page-break-before: always;
        }


        .pagebreak-after {
            page-break-after: always;
        }

        .card {
            position: relative;
            display: -ms-flexbox;
            display: flex;
            -ms-flex-direction: column;
            flex-direction: column;
            min-width: 0;
            word-wrap: break-word;
            background-color: #fff;
            background-clip: border-box;
            border: 1px solid rgba(0,0,0,.125);
            border-radius: 0px;
        }

        .accordion > .card {
            overflow: hidden;
        }

            .accordion > .card:first-of-type {
                border-bottom: 0;
                border-bottom-right-radius: 0;
                border-bottom-left-radius: 0;
            }



        .card-header {
            padding: 0px;
            margin-bottom: 0;
            background-color: rgba(0,0,0,.03);
            border-bottom: 1px solid rgba(0,0,0,.125);
        }

            .card-header:first-child {
                border-radius: 0px;
            }


        .accordion > .card .card-header {
            margin-bottom: -1px;
        }

        .card-body {
            -ms-flex: 1 1 auto;
            flex: 1 1 auto;
            padding: 1.25rem;
            font-size: 13px;
        }

        .hr {
             margin-top:2px; 
             margin-bottom:2px; 
            border: 0;
            border-top: 1px solid #eee
        }

        .header-small-font {
            font-size: 12px;
        }

        .gray {
            color: #777;
        }

        .rowInfo {
            padding-top: 0px;
            padding-bottom: 0px;
        }

        .mb-0 {
            margin: 2px;
        }
    </style>

    <div id="content" class="main-content">
        <h1 class="text-left text-primary">
            <label class="PageTitle"><%= GetLocalResourceObject("lblScreenTitle") %></label>
        </h1>
        <br />
        <asp:UpdatePanel runat="server" ID="upnMain">
            <Triggers>
            </Triggers>
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row">
            <div class="col-xs-4">
                <button type="button" class="btn btn-default btnAjaxAction"
                    data-loading-text="<span class='fa fa-spinner fa-spin glyphicon-main-button'></span> <%= GetLocalResourceObject("btnPrint") %>"
                    name="cmd" id="cmd">
                    <%= GetLocalResourceObject("btnPrint") %></button>
            </div>
        </div>


        <br />
        <div class="accordion" id="ResumenEncuesta" role="tablist" aria-multiselectable="true">
            <!-- card1-->
            <%-- Modulo Objetivo --%>
            <div class="card">
                <div class="card-header" id="collapseH1">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse1" aria-controls="collapse1">
                            <%=GetLocalResourceObject("txtTitleSocioEconomic") %>
                        </a>
                    </h2>
                </div>

                <div id="collapse1" class="collapse OpenToPrint" aria-labelledby="collapseH1" role="tabpanel" aria-labelledby="collapseH1">
                    <div class="card-body">
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("txtFechaStart") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="SurveyStartDateTimeFormatted">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%=GetLocalResourceObject("txtCompany") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="CompanyName">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("txtFarm") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="CostFarmName">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%=GetLocalResourceObject("txtDepartamento") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="DepartmentName">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("txtLabor") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="PositionName">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("txtFechaingreso") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="HireDateFormatted">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("txtTel") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="FullOfficePhone">...</span></div>
                            </div>
                            <%--    <div class="col col-sm-12">
                  <div class="col">Compañía</div>
                  <div class="col"><label id="Company">...</label></div>
              </div>--%>
                        </div>
                        <!-- end row-->



                    </div>
                </div>
            </div>
            <!-- end card-->
            <%-- Modulo Perfil Personal --%>
            <!-- card2-->
            <div class="card">
                <div class="card-header" id="collapseH2">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse2" aria-controls="collapse2">

                            <%= GetLocalResourceObject("txtStep1") %>
        
                        </a>
                    </h2>
                </div>
                <div id="collapse2" class="collapse OpenToPrint" aria-labelledby="collapseH2" role="tabpanel" aria-labelledby="collapseH2">
                    <div class="card-body">
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionID1") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="EmployeeID">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionID2") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="EmployeeName">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionID3") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="BirthDateFormatted">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionID4") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="Age">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionID5") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="NationalityName">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong id="BirthProvinceNamelbl" runat="server"></strong></div>
                                <div class="col-xs-7"><span class="gray" id="BirthProvinceName">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionID7") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="Gender">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_8") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_274">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->       
                    </div>
                </div>
            </div>
            <!-- end card-->
            <%--<div class="pagebreak-before"></div>--%>
            <%--<div class="pagebreak-after"></div>--%>
            <%-- Modulo Perfil Academico --%>
            <!-- card3-->
            <div class="card">
                <div class="card-header" id="collapseH3">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse3" aria-controls="collapse3">
                            <%= GetLocalResourceObject("txtStep2") %>
        
                        </a>
                    </h2>
                </div>

                <div id="collapse3" class="collapse OpenToPrint" aria-labelledby="collapseH3" role="tabpanel" aria-labelledby="collapseH3">
                    <div class="card-body">

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-7"><strong><%= GetLocalResourceObject("QuestionCode_9") %></strong></div>
                                <div class="col-xs-5"><span class="gray"></span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <br />

                        <!-- row-->
                        <div class="row rowInfo ">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_9_1") %></strong></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-8">
                                    <strong><%= GetLocalResourceObject("read") %>:</strong>
                                    <span class="gray" id="QuestionCode_188">...</span> &nbsp; 
                                    <strong><%= GetLocalResourceObject("write") %>:</strong>
                                    <span class="gray" id="QuestionCode_189">...</span>
                                    <br />                                    
                                </div>
                            </div>

                        </div>
                        <!-- end row-->

                        <br />

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_10") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_190">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_10_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_191">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                         <!-- row-->
                        <div class="row rowInfo">                            
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_11") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_192">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>


                        <!-- row-->
                        <div class="row rowInfo ">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_12") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_193">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5">
                                    <strong><%= GetLocalResourceObject("QuestionCode_12_1") %></strong>
                                    <span class="gray" id="QuestionCode_194">...</span>
                                </div>
                                <div class="col-xs-5">
                                    <strong><%= GetLocalResourceObject("QuestionCode_12_2") %></strong>
                                    <span class="gray" id="QuestionCode_195">...</span>
                                </div>

                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                          <!-- row-->
                        <div class="row rowInfo ">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_13") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_196">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_13_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_197">...</span></div>

                            </div>
                        </div>
                        <!-- end row-->
                    </div>
                </div>
            </div>
            <!-- end card-->

            <%--<div class="pagebreak-before"></div>--%>
            <%--<div class="pagebreak-after"></div>--%>
             <%-- Modulo Informacion Familiar --%>
            <!-- card4-->
            <div class="card">
                <div class="card-header" id="collapseH4">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse4" aria-controls="collapse4">

                            <%= GetLocalResourceObject("txtStep3") %>
        
                        </a>
                    </h2>
                </div>

                <div id="collapse4" class="collapse OpenToPrint" aria-labelledby="collapseH4" role="tabpanel">
                    <div class="card-body">

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                            </div>
                            <div class="col-xs-4 col-sm-4">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("NMen") %></strong></div>
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("NWoman") %></strong></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_14") %></strong></div>

                            </div>

                            <div class="col-xs-4 col-sm-4">

                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_199">..</span></div>
                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_200"></span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_15") %></strong></div>

                            </div>

                            <div class="col-xs-4 col-sm-4">

                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_202">..</span></div>
                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_203">..</span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_16") %></strong></div>

                            </div>

                            <div class="col-xs-4 col-sm-4">

                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_205">..</span></div>
                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_206"></span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_17") %></strong></div>

                            </div>

                            <div class="col-xs-4 col-sm-4">

                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_208">..</span></div>
                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_209"></span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_18") %></strong></div>

                            </div>

                            <div class="col-xs-4 col-sm-4">

                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_211">..</span></div>
                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_212">..</span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-8 col-sm-8">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_19") %></strong></div>

                            </div>

                            <div class="col-xs-4 col-sm-4">

                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_214">..</span></div>
                                <div class="col-xs-6 text-center"><span class="gray" id="QuestionCode_215">..</span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row">
                             <div class="col-xs-12">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_20_1") %></strong></div>                             
                             </div>

                            <table class="table table-bordered table-striped table-condensed header-small-font">

                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_a") %></th>                                        
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_b") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_c") %>	</th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_d") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_e") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_f") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_g") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_h") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_i") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_j") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_k") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_1_l") %></th>
                                    </tr>
                                </thead>

                                <tbody id="TablaQ20_1">
                                </tbody>
                            </table>  
                        </div>
                        <!-- end row-->
                         <div class="hr"></div>
                        <!-- row-->
                        <div class="row">
                             <div class="col-xs-10 col-sm-10">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionCode_20_2") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_228">...</span></div>
                            </div>
                           
                            <table class="table table-bordered table-striped table-condensed  header-small-font ">

                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_a") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_b") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_c") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_d") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_e") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_f") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_g") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_h") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_i") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_j") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_k") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_l") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_m") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_20_2_n") %></th>
                                    </tr>
                                </thead>

                                <tbody id="TablaQ20_2">
                                </tbody>
                            </table>
                             <div class="row">
                                            <asp:ListView runat="server" ID="lvHouseholdContributionRanges" GroupItemCount="6" >
                                                <LayoutTemplate>
                                                    <table runat="server" style="width:100%" id="tbHouseholdContributionRanges">
                                                        <tr runat="server" id="groupPlaceholder"></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <GroupTemplate>
                                                    <tr runat="server" id="tableRow" class="row">
                                                        <td runat="server" id="itemPlaceholder" />
                                                    </tr>
                                                </GroupTemplate>
                                                <ItemTemplate>
                                                    <td runat="server" class="col-sm-2" style="padding-top:20px;">                                                        
                                                        <asp:Label ID="lblHouseholdContributionRange" runat="server" Text='<%#Eval("RangeFormated") %>' />
                                                    </td>
                                                </ItemTemplate>
                                            </asp:ListView>
                             </div>
                             <div class="hr"></div>

                        </div>
                        <!-- end row-->
                        <!--  row-->
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_21") %></strong></div>                             
                             </div>
                            <table class="table table-bordered table-striped table-condensed header-small-font">

                                <thead>
                                    <tr>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_a") %></th>
                                        <th><%= GetLocalResourceObject("informativeText") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_b") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_c") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_d") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_e") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_f") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_g") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_h") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_i") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_j") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_21_k") %></th>
                                    </tr>
                                </thead>

                                <tbody id="TablaQ21">
                                </tbody>
                            </table>
                            <div class="row">
                                            <asp:ListView runat="server" ID="lvHouseholdContributionRangesCopy" GroupItemCount="6" >
                                                <LayoutTemplate>
                                                    <table runat="server" style="width:100%" id="tbHouseholdContributionRanges">
                                                        <tr runat="server" id="groupPlaceholder"></tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <GroupTemplate>
                                                    <tr runat="server" id="tableRow" class="row">
                                                        <td runat="server" id="itemPlaceholder" />
                                                    </tr>
                                                </GroupTemplate>
                                                <ItemTemplate>
                                                    <td runat="server" class="col-sm-2" style="padding-top:20px;">                                                        
                                                        <asp:Label ID="lblHouseholdContributionRange" runat="server" Text='<%#Eval("RangeFormated") %>' />
                                                    </td>
                                                </ItemTemplate>
                                            </asp:ListView>
                             </div>
                             <div class="hr"></div>
                        </div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_22") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_253">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_22_1") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_254">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <%--<div class="pagebreak-before"></div>--%>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23") %></strong></div>
                                <div class="col-xs-4"><span class="text-danger" runat="server"><%= GetLocalResourceObject("InfoMessage_23") %></span></div>
                                <div class="col-xs-4"><span class="text-danger" id="lblCurrency" runat="server">...</span></div>
                            </div>

                        </div>
                        <!-- end row-->

                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_1") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_256">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_2") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_257">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_3") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_258">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_4") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_259">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->

                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_8") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_266">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_9") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_267">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_10") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_268">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_11") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_269">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->

                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_12") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_270">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_13") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_271">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_14") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_272">...</span></div>
                            </div>
                            <div class="col-xs-3 col-sm-3">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_23_15") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_273">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->

                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23_5_a") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_260">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23_5_b") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_261">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->


                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23_6_a") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_262">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23_6_b") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_263">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->

                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23_7_a") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_264">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_23_7_b") %></strong></div>
                                <div class="col-xs-8"><span class="gray" id="QuestionCode_265">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                    </div>
                </div>
            </div>
            <!-- end card-->
            <%-- Modulo Salud y Bienestar Familiar --%>

            <%--<div class="pagebreak-before"></div>--%>
            <%--<div class="pagebreak-after"></div>--%>
            <!-- card5-->
            <div class="card">
                <div class="card-header" id="collapseH5">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse5" aria-controls="collapse5">

                            <%= GetLocalResourceObject("Step4") %>
        
                        </a>
                    </h2>
                </div>

                <div id="collapse5" class="collapse OpenToPrint" aria-labelledby="collapseH5" role="tabpanel" aria-labelledby="collapseH5">
                    <div class="card-body">

                          <!-- row-->
                        <div class="row">
                             <div class="col-xs-12">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_24") %></strong></div>                             
                             </div>

                            <table class="table table-bordered table-striped table-condensed">

                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th><%= GetLocalResourceObject("informativeText") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_24_2") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_24_3") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_24_4") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_24_5") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_24_6") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_24_7") %></th>
                                    </tr>
                                </thead>

                                <tbody id="TablaQ24">
                                </tbody>
                            </table>
                        </div>
                        <!-- end row-->
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_25") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_283"></span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_25_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_284">...</span></div>
                                <br/>
                            </div>
                            <div>
                                <span><br /></span>
                            </div>
                            <div class="col-xs-6 col-sm-6">                               
                            </div>
                             <div>
                                <span><br /></span>
                            </div>
                             <div class="col-xs-6 col-sm-6" id="Question_25_2" runat="server">
                                 <span><br /></span>
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_25_2") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_285">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                             <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionCode_26_a") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="QuestionCode_286">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_26_b") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_287">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <br />
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_26_1") %></strong></div>                             
                             </div>
                            <table class="table table-bordered table-striped table-condensed">

                                <thead>
                                    <tr>                                        <th>#</th>
                                        <th><%= GetLocalResourceObject("QuestionCode_26_1_a") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_26_1_b") %></th>
                                    </tr>
                                </thead>
                                <tbody id="TablaQ26">
                                </tbody>
                            </table>
                        </div>
                        <div class="hr"></div>
                        <%--<div class="pagebreak-after"></div>--%>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-10"><strong><%= GetLocalResourceObject("QuestionCode_27") %></strong></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <br />
                        <div class="row">
                            <table class="table table-bordered table-striped table-condensed">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th colspan="2" class="text-center"><%= GetLocalResourceObject("QuestionCode_27_Header") %></th>
                                    </tr>
                                    <tr>
                                        <th class="text-center"><%= GetLocalResourceObject("QuestionCode_27_a") %></th>
                                        <th class="text-center"><%= GetLocalResourceObject("QuestionCode_27_b") %></th>
                                        <th class="text-center"><%= GetLocalResourceObject("QuestionCode_27_c") %></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><%= GetLocalResourceObject("QuestionCode_27_a_1") %></td>
                                        <td class="text-center"><span class="gray" id="QuestionCode_293_1"></span></td>
                                        <td class="text-center"><span class="gray" id="QuestionCode_294_1"></span></td>
                                    </tr>
                                    <tr>
                                        <td><%= GetLocalResourceObject("QuestionCode_27_a_3") %></td>
                                        <td class="text-center"><span class="gray" id="QuestionCode_293_2"></span></td>
                                        <td class="text-center"><span class="gray" id="QuestionCode_294_2"></span></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <%--<div class="pagebreak-before"></div>--%>
                        <%--<div class="pagebreak-after"></div>--%>
                       
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-11"><strong><%= GetLocalResourceObject("QuestionCode_28") %></strong></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <br />
                        <div class="row">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"></div>
                                <div class="col-xs-2"><strong><%= GetLocalResourceObject("QuestionCode_28_a") %></strong></div>
                                <div class="col-xs-2"><strong><%= GetLocalResourceObject("lblSpecify") %></strong></div>
                                <div class="col-xs-1"><strong><%= GetLocalResourceObject("QuestionCode_28_b") %></strong></div>
                                <div class="col-xs-2"><strong><%= GetLocalResourceObject("lblSpecify") %></strong></div>
                            </div> 
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-1"> </div>
                                <div class="col-xs-4 "><strong><%= GetLocalResourceObject("QuestionCode_28_1") %></strong></div>
                                <div class="col-xs-2"><span class="gray" id="QuestionCode_298">...</span></div>
                                <div class="col-xs-2"><span class="gray" id="QuestionCode_299">...</span></div>
                                <div class="col-xs-1"><span class="gray" id="QuestionCode_300">...</span></div>
                                <div class="col-xs-2"><span class="gray" id="QuestionCode_301">...</span></div>
                            </div> 
                        </div>
                        <div class="hr"></div>
                        
                        <%--<div class="pagebreak-after"></div>--%>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_29") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_302">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-4"><strong><%= GetLocalResourceObject("QuestionCode_29_1") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_303">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                    </div>
                </div>
            </div>
            <!-- end card-->
            <%-- Modulo Bienestar Familiar --%>
            <!-- card6-->
            <div class="card">
                <div class="card-header" id="collapseH6">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse6" aria-controls="collapse6">
                            <%= GetLocalResourceObject("Step5text") %>        
                        </a>
                    </h2>
                </div>

                <div id="collapse6" class="collapse OpenToPrint" aria-labelledby="collapseH6" role="tabpanel" aria-labelledby="collapseH6">
                    <div class="card-body">
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-7 col-sm-7">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_30") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_304"></span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-7 col-sm-7">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_31") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_305"></span></div>
                            </div>
                        </div>
                        <!-- end row-->
                    </div>
                </div>
            </div>
            <!-- end card-->
             <%-- Modulo Vivienda Familiar --%>
            <!-- card7-->
            <div class="card">
                <div class="card-header" id="collapseH7">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse7" aria-controls="collapse7">
                            <%= GetLocalResourceObject("Step6text") %>        
                        </a>
                    </h2>
                </div>

                <div id="collapse7" class="collapse OpenToPrint" aria-labelledby="collapseH7" role="tabpanel" aria-labelledby="collapseH7">
                    <div class="card-body">
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_32") %></strong></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-7"><strong id="lblProvince" runat="server"><%--<%= GetLocalResourceObject("QuestionCode_116") %>--%> </strong></div>
                                <div class="col-xs-5"><span class="gray" id="QuestionCode_308"></span></div>
                            </div>

                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-7"><strong id="lblCanton" runat="server"><%--<%= GetLocalResourceObject("QuestionCode_117") %>--%></strong></div>
                                <div class="col-xs-5"><span class="gray" id="QuestionCode_309">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <div class="row rowInfo">

                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-7"><strong id="lblDistrict" runat="server"><%--<%= DivisionCodeGlobal==5? GetLocalResourceObject("QuestionCode_118_Ecu") : GetLocalResourceObject("QuestionCode_118")  %>--%></strong></div>
                                <div class="col-xs-5"><span class="gray" id="QuestionCode_310">...</span></div>
                            </div>

                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-7"><strong id="lblNeighborhood" runat="server"><%--<%= DivisionCodeGlobal==5? GetLocalResourceObject("QuestionCode_119_Ecu") : GetLocalResourceObject("QuestionCode_119") %>--%></strong></div>
                                <div class="col-xs-5"><span class="gray" id="QuestionCode_311">N/A </span></div>
                            </div>
                        </div>
                        <div class="hr"></div>
                         <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_32_1_e") %> </strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_312"></span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionCode_32_2") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="QuestionCode_313">...</span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionCode_33") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="QuestionCode_314">...</span></div>
                            </div>

                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_34") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_315">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->

                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionCode_35") %></strong></div>
                                <div class="col-xs-6"><span class="gray" id="QuestionCode_316">...</span></div>
                            </div>

                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_36") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_317">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_37") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_318">...</span></div>
                            </div>


                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_38") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_319">...</span></div>
                            </div>
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-8"><strong><%= GetLocalResourceObject("QuestionCode_38_1") %></strong></div>
                                <div class="col-xs-4"><span class="gray" id="QuestionCode_320">...</span></div>
                            </div>


                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_39") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_321"></span></div>
                            </div>


                        </div>
                        <!-- end row-->

                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong style="margin-left: 30px;"><%= GetLocalResourceObject("QuestionCode_39_a") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_322">...</span></div>
                            </div>

                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_39_b") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_323">...</span></div>
                            </div>

                        </div>
                        <!-- end row-->
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong style="margin-left: 30px;"><%= GetLocalResourceObject("QuestionCode_39_c") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_352">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-6"><strong><%= GetLocalResourceObject("QuestionCode_40") %></strong></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_40_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_325">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_40_2") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_327">...</span>
                             </div>
                            </div>
                          
                        </div>
                        <!-- end row-->                          
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_40_3") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_330">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                         <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_40_4") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_332">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-6 col-sm-6">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_41") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_333">...</span></div>
                            </div>                            
                        </div>
                        <!-- end row-->
                      
                        <div class="hr"></div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-12"><strong><%= GetLocalResourceObject("QuestionCode_41_1") %></strong></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <br />

                        <div class="row">
                            <table class="table table-bordered table-striped table-condensed">

                                <thead>
                                    <tr>
                                        <th><%= GetLocalResourceObject("QuestionCode_41_1_a") %></th>
                                        <th><%= GetLocalResourceObject("QuestionCode_41_1_b") %></th>

                                    </tr>
                                </thead>

                                <tbody id="TablaQ41">
                                </tbody>


                            </table>

                        </div>
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_42") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_337"></span></div>
                            </div>


                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_42_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_338"></span></div>
                            </div>


                        </div>
                        <!-- end row-->
                        <div class="hr"></div>


                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_43") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_339"></span></div>
                            </div>


                        </div>
                        <!-- end row-->
                        <div class="hr"></div>


                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_44") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_340"></span></div>
                            </div>


                        </div>
                        <!-- end row-->
                        <div class="hr"></div>


                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_45") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_341"></span></div>
                            </div>
                           
                        </div>
                        <!-- end row-->
                        <div>
                            <span><br /></span>
                        </div>
                        <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_45_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_342">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                        <div class="hr"></div>

                        <!-- row-->
                        <div class="row rowInfo">
                             <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_46") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_343"></span></div>
                            </div>
                        </div>
                        <!-- end row-->
                         <div>
                            <span><br /></span>
                        </div>
                          <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_46_a") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_344">...</span></div>
                            </div>
                        </div>
                        <!-- end row-->
                         <div>
                            <span><br /></span>
                        </div>
                            <!-- row-->
                        <div class="row rowInfo">
                             <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-5"><strong><%= GetLocalResourceObject("QuestionCode_46_1") %></strong></div>
                                <div class="col-xs-7"><span class="gray" id="QuestionCode_345"></span></div>
                            </div>
                        </div>
                        <!-- end row-->

                    </div>
                </div>
            </div>
            <!-- end card-->

             <!-- end card-->
            <%-- Fin encuesta --%>
            <!-- card8-->
            <div class="card">
                <div class="card-header" id="collapseH8">
                    <h2 class="mb-0">
                        <a class="btn btn-link" type="button" aria-expanded="true" data-toggle="collapse" data-parent="#ResumenEncuesta"
                            href="#collapse8" aria-controls="collapse8">

                            <%= GetLocalResourceObject("Step7text") %>
        
                        </a>
                    </h2>
                </div>

                <div id="collapse8" class="collapse OpenToPrint" aria-labelledby="collapseH8" role="tabpanel" aria-labelledby="collapseH8">
                    <div class="card-body">  
                          <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-3"><strong><%= GetLocalResourceObject("observation") %></strong></div>
                                <div class="col-xs-9"><div class="linea"></div></div>
                                <div class="col-xs-12"><div class="linea"></div></div>
                                <div class="col-xs-12"><div class="linea"></div></div>
                                <div class="col-xs-12"><div class="linea"></div></div>
                                <div class="col-xs-12"><div class="linea"></div></div>
                                <div class="col-xs-12"><div class="linea"></div></div>
                                
                            </div>   
                        </div>
                        <br />
                         <!-- row-->
                        <div class="row rowInfo">
                             <div class="col-xs-12 col-sm-12">
                                
                            </div>
                        </div>
                         <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-3"><strong><%= GetLocalResourceObject("informedConsent") %></strong></div>
                                <div class="col-xs-9"> <%= GetLocalResourceObject("consentText") %> </div>
                            </div>
                        </div>
                        <br />
                         <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-6"><%= GetLocalResourceObject("signJob") %> </div>
                                <div class="col-xs-6"> <%= GetLocalResourceObject("completeBy") %> </div>
                            </div>
                        </div>
                        <br />
                         <!-- row-->
                        <div class="row rowInfo">
                            <div class="col-xs-12 col-sm-12">
                                <div class="col-xs-6" ><%= GetLocalResourceObject("finalHour") %> <span id="finalHour"></span> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- fin accordion-->


    </div>

    <%--  Modal User Search  --%>
    <div class="modal fade" id="employeeSearchDialog" tabindex="-1" role="dialog" aria-labelledby="employeeSearchTitle" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <strong>
                        <h3 class="modal-title text-primary text-center" id="employeeSearchDialogTitle"><%= GetLocalResourceObject("lblEmployeeSearchTitle.Text") %></h3>
                    </strong>
                </div>
                <asp:UpdatePanel runat="server" ID="upnEmployeeSearch">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <div class="col-sm-4">
                                                <asp:Label ID="lblSearchEmpoyeeCode" meta:resourcekey="lblSearchEmpoyeeCode" AssociatedControlID="txtEmployeeCodeSearch" runat="server" Text="" CssClass="text-left"></asp:Label>
                                            </div>
                                            <div class="col-sm-5">
                                                <asp:TextBox ID="txtEmployeeCodeSearch" MaxLength="20" TabIndex="1" CssClass="form-control control-validation cleanPasteText" onkeypress="return isNumberOrLetterNoEnter(event);" runat="server" meta:resourcekey="txtEmployeeCodeSearch" placeholder=""></asp:TextBox>
                                                <label id="lblSearchEmployeeCodeValidation" for="<%= txtEmployeeCodeSearch.ClientID%>" class="label label-danger label-validation" data-toggle="tooltip" data-container="body" data-placement="left" data-content="<%= GetLocalResourceObject("msjEmployeeSearchValidation") %>" style="display: none; float: right; margin-right: 6px; margin-top: -23px; position: relative; z-index: 2;">!</label>
                                            </div>
                                            <div class="col-sm-2">
                                                <button id="btnEmployeeSearch" type="button" runat="server" tabindex="2" class="btn btn-primary btnAjaxAction" onclick="return ProcessEmployeeSearchRequest(this.id);" onserverclick="btnEmployeeSearch_ServerClick"
                                                    data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'
                                                    data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span>&nbsp;", GetLocalResourceObject("btnSearch"))%>'>
                                                    <span class="glyphicon glyphicon-search glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span>&nbsp;<%=Convert.ToString(GetLocalResourceObject("btnSearch")) %>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <asp:HiddenField runat="server" ID="hdfSelectedRowIndex" Value="-1" />
                                <div class="scrolling-table-container-vertical">
                                    <asp:GridView ID="grvEmployees" CssClass="table table-bordered table-striped table-hover" ShowHeader="true"
                                        OnPreRender="grvEmployees_PreRender" Width="100%" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        EmptyDataRowStyle-CssClass="emptyRow" PagerSettings-Visible="false" AllowSorting="false"
                                        DataKeyNames="EmployeeCode,GeographicDivisionCode" ShowHeaderWhenEmpty="True" EmptyDataText='<%$ Code:GetLocalResourceObject("lblNoData") %>'>
                                        <Columns>
                                            <asp:BoundField DataField="EmployeeCode"
                                                ItemStyle-Width="25%"
                                                HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeCodeHeaderText") %>' />
                                            <asp:BoundField DataField="GeographicDivisionCode"
                                                HeaderText="GeographicDivisionCode"
                                                ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" FooterStyle-CssClass="hidden" />
                                            <asp:BoundField DataField="ID" ItemStyle-Width="25%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeIDHeaderText") %>' />
                                            <asp:BoundField DataField="EmployeeName" ItemStyle-CssClass="fullNameGet"
                                                ItemStyle-Width="50%" HeaderText='<%$ Code:GetLocalResourceObject("grvEmployeesEmployeeFullNameHeaderText") %>' />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnAcceptSelectedEmployee" runat="server" CssClass="btn btn-default btnAjaxAction"
                                TabIndex="6"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnAcceptSelectedEmployee.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnAcceptSelectedEmployee.Text"))%>'>
                                <span class="glyphicon glyphicon-ok glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                <%= GetLocalResourceObject("lbtnAcceptSelectedEmployee.Text") %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lbtnCancelEmployeeSearch" runat="server"
                                TabIndex="7" CssClass="btn btn-default btnAjaxAction" OnClick="lbtnCancelEmployeeSearch_Click" OnClientClick="return ProcessCancelEmployeeSearchRequest(this.id);"
                                data-loading-text='<%$ Code:String.Concat("<span class=\"fa fa-spinner fa-spin glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancelEmployeeSearch.Text"))%>'
                                data-error-text='<%$ Code:String.Concat("<span class=\"glyphicon glyphicon-exclamation-sign glyphicon-main-button\"></span><br />", GetLocalResourceObject("lbtnCancelEmployeeSearch.Text"))%>'>
                                <span class="glyphicon glyphicon-remove glyphicon-main-button" aria-hidden="true" aria-label="main-buttons"></span><br />
                                <%= GetLocalResourceObject("lbtnCancelEmployeeSearch.Text") %>
                            </asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <script>
        var datos;
        var PersonalInfo;
        var CompletedForm;
        var TableQuestion20_1;
        var TableQuestion20_2;
        var TableQuestion21;
        var TableQuestion24;
        var TableQuestion26;
        var TableQuestion28;
        var TableQuestion41;

        var MultipleCombo = -1;

        var addRowQ21;
        var addRowQ24;
        var addRowQ26;
        var addRowQ28;
        var addRowQ41;
        var employeeCodSelect = "";
        var employeeFullName = "";
        let dollarUSLocale = Intl.NumberFormat('en-US');
        $(document).ready(function () {




            $('#cmd').click(function () {
                var $this = this;
                $($this).button('loading');
                $('.OpenToPrint').collapse('show');
                //   $(".pagebreak").css("height", "5px");

                setTimeout(function () {
                    var element = document.getElementById('ResumenEncuesta');

                    var opt = {
                        margin: [14, 1, 6, 1],
                        pagebreak: { mode: ['avoid-all', 'css', 'legacy'], before: '.pagebreak-before', after: '.pagebreak-after' },
                        filename:     '<%=GetLocalResourceObject("PdfName")%>' + employeeCodSelect + '.pdf',
                        image: { type: 'jpeg', quality: 0.98 },
                        //html2canvas:  { scale: 2 },
                        jsPDF: {
                            // unit: 'in',
                            format: 'letter',
                            orientation: 'landscape'
                        },

                    };

                    // Old monolithic-style usage:
                    //    html2pdf(element, opt);
                    html2pdf().from(element).set(opt).toPdf().get('pdf').then(function (pdf) {

                        var totalPages = pdf.internal.getNumberOfPages();

                        for (i = 1; i <= totalPages; i++) {

                            pdf.setPage(i);


                            //  pdf.text('text 2 I am on page' + i, 10, 10);
                            pdf.addImage(imgData, "PNG", 2, 2, 15, 11);
                            pdf.setFontSize(15);
                            pdf.setTextColor(100);
                            pdf.text(employeeCodSelect + " - " + employeeFullName, pdf.internal.pageSize.getWidth() - 210, 8)

                            pdf.setFontSize(10); pdf.setTextColor(150);
                            pdf.text('<%=GetLocalResourceObject("Page")%> ' + i + ' <%=GetLocalResourceObject("Of")%> ' + totalPages, pdf.internal.pageSize.getWidth() - 155, pdf.internal.pageSize.getHeight() - 3);
        //  pdf.addImage(imgData, "PNG", 20, 20, 20, 20);
    }


}).save();

                    $($this).button('reset');
                }, 3000)

            });


            PersonalInfo = function (Survey) {
                //ficha economica
                $("#SurveyStartDateTimeFormatted").text(Survey.SurveyStartDateTimeFormatted);
                $("#CompanyName").text(Survey.CompanyName);
                $("#CostFarmName").text(Survey.CostFarmName);
                $("#DepartmentName").text(Survey.DepartmentName);
                $("#PositionName").text(Survey.PositionName);
                $("#HireDateFormatted").text(Survey.HireDateFormatted);
                $("#FullOfficePhone").text(Survey.FullOfficePhone);
                //personal Info
                $("#EmployeeID").text(Survey.EmployeeID);
                $("#EmployeeName").text(Survey.EmployeeName);
                $("#BirthDateFormatted").text(Survey.BirthDateFormatted);
                $("#Age").text(Survey.Age);
                $("#Gender").text(Survey.Gender);
                $("#NationalityName").text(Survey.NationalityName);
                $("#BirthProvinceName").text(Survey.BirthProvinceName);
                //finalHour
                $("#finalHour").text(Survey.SurveyEndDateTimeFormatted);


            }

            var QuestionCode_110_35 = null;
            CompletedForm = function (Answers) {
                
                Answers.forEach(function (item) {

                    switch (parseInt(item.QuestionCode)) {

                        //Combos Select Text
                        case 190:
                        case 194:
                        case 196:
                        case 274:
                        case 284:
                        case 308:
                        case 309:
                        case 310:
                        case 311:
                        case 313:
                        case 315:
                        case 316:
                        case 317:
                        case 325:
                        case 327:
                        case 330:

                            $("#QuestionCode_" + item.QuestionCode).text(item.AnswerText);
                            break;
                        //bool text
                        case 193:
                        case 192:
                        case 188:
                        case 189:
                        case 253:
                        case 283:
                        case 285:
                        case 286:
                        case 302:
                        case 318:
                        case 319:
                        case 314:
                        case 332:
                        case 338:
                        case 342:
                        case 345:
                            
                            var BoolText = item.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>";
                           //QuestionCode_110_35 = item.QuestionCode == 107 ? BoolText : "";
                            $("#QuestionCode_" + item.QuestionCode).text(BoolText);
                            break;
                        //multiplecomboSelect
                        case 197:
                        case 304:
                        case 299:
                        case 301:
                        case 305:
                        case 321:
                        case 333:
                        case 337:
                        case 339:
                        case 340:
                        case 341:
                        case 343:

                            var Texto = item.QuestionCode == MultipleCombo ? ", " + item.AnswerText : item.AnswerText;

                            MultipleCombo = item.QuestionCode;
                            $("#QuestionCode_" + item.QuestionCode).append(Texto);

                            MultipleCombo = item.QuestionCode;

                            break;

                        case 293:
                        case 294: $("#QuestionCode_" + item.QuestionCode + "_" + item.AnswerItem).text(item.AnswerValue);
                            break;

                        case 169:
                            if (item.QuestionCode != null && item.QuestionCode != "") {
                                $("#QuestionCode_" + item.QuestionCode).append("N/A");
                            } else {

                                $("#QuestionCode_" + item.QuestionCode).append("N/A");
                            }

                            break;

                        default:
                            item.QuestionCode = parseInt(item.QuestionCode);                            
                            if (item.QuestionCode >= 217 && 227 >= item.QuestionCode) {
                                TableQuestion20_1(item);
                            } if ((item.QuestionCode >= 229 && 240 >= item.QuestionCode) || (item.QuestionCode >= 346 && 349 >= item.QuestionCode)) {
                                TableQuestion20_2(item);
                            } else if ((item.QuestionCode >= 242 && 252 >= item.QuestionCode) || (item.QuestionCode == 351)) {
                                TableQuestion21(item);
                            } else if (item.QuestionCode >= 277 && 282 >= item.QuestionCode) {
                                TableQuestion24(item);
                            } else if (item.QuestionCode >= 289 && 290 >= item.QuestionCode) {
                                TableQuestion26(item);
                            } else if (item.QuestionCode >= 335 && 336 >= item.QuestionCode) {
                                TableQuestion41(item);
                            } else if (item.QuestionCode >= 256 && 273 >= item.QuestionCode) {
                                if (item.QuestionCode == 261 || item.QuestionCode == 263 || item.QuestionCode == 265) {
                                    $("#QuestionCode_" + item.QuestionCode).text(item.AnswerValue);
                                } else {
                                    $("#QuestionCode_" + item.QuestionCode).text(dollarUSLocale.format(item.AnswerValue));
                                }
                            } else {
                                if (item.QuestionCode == 111) {
                                    if (QuestionCode_110_35 != "<%= GetLocalResourceObject("not")%>")
                                        $("#QuestionCode_" + item.QuestionCode).text(item.AnswerValue);
                                } else {
                                    $("#QuestionCode_" + item.QuestionCode).text(item.AnswerValue);
                                }

                            }

                            break;
                    }

                });//foreach
            }//end void
            var IRowQ20_1 = 1;

            addRowQ20_1 = function (RowNumber) {
                var row = "<tr id='Q20_1Row_" + RowNumber + "'>" +

                    "<td >" + IRowQ20_1 + "</td>" +                    
                    "<td id='Q20_1_RWTI_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_Gender_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_Age_" + RowNumber + "' class='text-right'></td>" +
                    "<td id='Q20_1_CS_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_RAW_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_LACD_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_CY_" + RowNumber + "'  class='text-right'></td>" +
                    "<td id='Q20_1_HaveDeg_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_CurrntStud_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_CurrDeg_" + RowNumber + "'></td>" +
                    "<td id='Q20_1_CurrYer_" + RowNumber + "'></td>"+
                    "<tr>";

                $("#TablaQ20_1").append(row);
                IRowQ20_1 = IRowQ20_1 + 1;
            }


            var IRowQ20_2 = 1;
            addRowQ20_2 = function (RowNumber) {
                var row = "<tr id='Q20_2Row_" + RowNumber + "'>" +

                    "<td >" + IRowQ20_2 + "</td>" +
                    "<td id='Q20_2_RWTI_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_Gender_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_Age_" + RowNumber + "' class='text-right'></td>" +
                    "<td id='Q20_2_CivilS_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_RAW_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_MDegre_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_YearComp_" + RowNumber + "'  class='text-right'></td>" +
                    "<td id='Q20_2_CS_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_CG_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_CY_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_Soci_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_Aid_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_WRK_" + RowNumber + "'></td>" +
                    "<td id='Q20_2_MenHelp_" + RowNumber + "'></td>" +
                    "<tr>";

                $("#TablaQ20_2").append(row);
                IRowQ20_1 = IRowQ20_1 + 1;
            }

            var IRowQ21 = 1;
            addRowQ21 = function (RowNumber) {
                var row = "<tr id='Q21Row_" + RowNumber + "'>" +
                    "<td >" + IRowQ21 + "</td>" +
                    "<td id='Q21_Info_" + RowNumber + "'></td>" +
                    "<td id='Q21_WRK_" + RowNumber + "'></td>" +
                    "<td id='Q21_WRKDOLE_" + RowNumber + "'  class='text-right'></td>" +
                    "<td id='Q21_HSS_" + RowNumber + "'></td>" +
                    "<td id='Q21_RHO_" + RowNumber + "'></td>" +
                    "<td id='Q21_CPSP_" + RowNumber + "'></td>" +
                    "<td id='Q21_AP_" + RowNumber + "'></td>" +
                    "<td id='Q21_CMH_" + RowNumber + "'></td>" +
                    "<td id='Q21_PPJ_" + RowNumber + "'></td>" +
                    "<td id='Q21_BOLRNW_" + RowNumber + "'></td>" +
                    "<td id='Q21_RNW_" + RowNumber + "'></td>" +
                    "<tr>";

                $("#TablaQ21").append(row);
                IRowQ21 = IRowQ21 + 1;
            }

            var IRowQ24 = 0;
            addRowQ24 = function (RowNumber) {
                var row = "<tr id='Q24Row_" + RowNumber + "'>" +
                    "<td >" + IRowQ24 + "</td>" +
                    "<td id='Q24_Info_" + RowNumber + "'></td>" +
                    "<td id='Q24_DiseCareP_" + RowNumber + "'></td>" +
                    "<td id='Q24_VisitMed_" + RowNumber + "'></td>" +
                    "<td id='Q24_ReasonNotVisit_" + RowNumber + "'></td>" +
                    "<td id='Q24_HaveProblem_" + RowNumber + "'></td>" +
                    "<td id='Q24_HaveAtentionMd_" + RowNumber + "'></td>" +
                    "<td id='Q24_WhyNotMEdic_" + RowNumber + "'></td>" +
                    "<tr>";

                $("#TablaQ24").append(row);
                IRowQ24 = IRowQ24 + 1;
            }

            var IRowQ26 = 1;
            addRowQ26 = function (RowNumber) {
                var row = "<tr id='Q26Row_" + RowNumber + "'>" +
                    "<td >" + IRowQ26 + "</td>" +
                    "<td id='Q26_RWTI_" + RowNumber + "'></td>" +
                    "<td id='Q26_Disability_" + RowNumber + "'></td>" +
                    "<tr>";

                $("#TablaQ26").append(row);
                IRowQ26 = IRowQ26 + 1;
            }

            addRowQ28 = function (RowNumber) {
                var row = "<tr id='Q28Row_" + RowNumber + "'>" +

                    "<td id='Q28_Disease_" + RowNumber + "'></td>" +
                    "<td id='Q28_Frenquency_" + RowNumber + "'></td>" +


                    "<tr>";

                $("#TablaQ28").append(row);
            }

            addRowQ41 = function (RowNumber) {
                var row = "<tr id='Q41Row_" + RowNumber + "'>" +
                    "<td id='Q41_Service_" + RowNumber + "'></td>" +

                    "<td id='Q41_Availability_" + RowNumber + "'></td>" +



                    "<tr>";

                $("#TablaQ41").append(row);
            }

            TableQuestion20_1 = function (Answers) {
                
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //Relationship to interviewee
                    case 217:
                        addRowQ20_1(RowNumber);
                        $("#Q20_1_RWTI_" + RowNumber).text(Answers.AnswerText);
                        break;
                    //genderColum
                    case 218: $("#Q20_1_Gender_" + RowNumber).text(Answers.AnswerValue);  break;
                    //AgeColum
                    case 219: $("#Q20_1_Age_" + RowNumber).text(Answers.AnswerValue);break;
                    //Civil status
                    case 220: $("#Q20_1_CS_" + RowNumber).text(Answers.AnswerText); break;
                    //Read and write
                    case 221: $("#Q20_1_RAW_" + RowNumber).text(Answers.AnswerValue == "True" ?"<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Years of study
                    case 222: $("#Q20_1_LACD_" + RowNumber).text(Answers.AnswerText); break;
                    //Maximum academic grade approved
                    case 223: $("#Q20_1_CY_" + RowNumber).text(Answers.AnswerValue); break;
                    // have a professional training course
                    case 224: $("#Q20_1_HaveDeg_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Currently studying
                    case 225: $("#Q20_1_CurrntStud_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Academic degree studying
                    case 226: $("#Q20_1_CurrDeg_" + RowNumber).text(Answers.AnswerText); break;
                    //Year of study
                    case 227: $("#Q20_1_CurrYer_" + RowNumber).text(Answers.AnswerValue); break;
                   
                }
            }
            TableQuestion20_2 = function (Answers) {
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //Relationship to interviewee
                    case 229:
                        addRowQ20_2(RowNumber);
                        $("#Q20_2_RWTI_" + RowNumber).text(Answers.AnswerText);
                        break;
                    //genderColum
                    case 230: $("#Q20_2_Gender_" + RowNumber).text(Answers.AnswerValue); break;
                    //AgeColum
                    case 231: $("#Q20_2_Age_" + RowNumber).text(Answers.AnswerValue); break;
                    //Civil status
                    case 232: $("#Q20_2_CivilS_" + RowNumber).text(Answers.AnswerText); break;
                    //Read and write
                    case 233: $("#Q20_2_RAW_" + RowNumber).text(Answers.AnswerValue == "True" ?"<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Degree
                    case 234: $("#Q20_2_MDegre_" + RowNumber).text(Answers.AnswerText); break;
                    //Coursing year
                    case 235: $("#Q20_2_YearComp_" + RowNumber).text(Answers.AnswerValue); break;
                    // Currently studying
                    case 236: $("#Q20_2_CS_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //current degree
                    case 237: $("#Q20_2_CG_" + RowNumber).text(Answers.AnswerText); break;
                    //Currently year
                    case 238: $("#Q20_2_CY_" + RowNumber).text(Answers.AnswerValue); break;
                    //Social security
                    case 240: $("#Q20_2_Soci_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //AAid, pension, other
                    case 346: $("#Q20_2_Aid_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Does he or she works?
                    case 347: $("#Q20_2_WRK_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //
                    case 348: $("#Q20_2_MenHelp_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //contribution house
                    case 349: $("#Q20_2_MenHelp_" + RowNumber).text($("#Q20_2_MenHelp_" + RowNumber).text() +" , "+ Answers.AnswerValue); break;
                }
            }

            TableQuestion21 = function (Answers) {
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //Does he or she works?
                    case 243:
                        addRowQ21(RowNumber);
                        $("#Q21_WRK_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); 

                        $("#Q21_Info_" + RowNumber).text($("#Q20_1_RWTI_" + RowNumber).text() + " , " + $("#Q20_1_Age_" + RowNumber).text());
                        break;
                    //Does this person work for a dole company?
                    case 244: $("#Q21_WRKDOLE_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Have social security (medical coverage)
                    case 245: $("#Q21_HSS_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Do you receive help, subsidies, others?
                    case 246: $("#Q21_RHO_" + RowNumber).text(Answers.AnswerValue == "True" ?"<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Is this person contributing/paying to any pension system or fund?
                    case 247: $("#Q21_CPSP_" + RowNumber).text(Answers.AnswerValue == "True" ?"<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Is this person already a pensioner?
                    case 248: $("#Q21_AP_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Does this person contribute monthly to the household?
                    case 249: $("#Q21_CMH_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //In their main occupation/job, this person is:
                    case 250: $("#Q21_PPJ_" + RowNumber).text(Answers.AnswerText); break;
                    //In the last 4 weeks, has this person done any paperwork to look for a job or set up their own business?
                    case 251: $("#Q21_BOLRNW_" + RowNumber).text(Answers.AnswerValue == "True" ?"<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //What was the main reason why this person did not carry out any procedure to look for a job or set up their own business in the last four weeks?
                    case 252: $("#Q21_RNW_" + RowNumber).text(Answers.AnswerText); break;
                    //Does this person contribute monthly to the household?
                    case 351: $("#Q21_CMH_" + RowNumber).text($("#Q21_CMH_" + RowNumber).text()+" , " + Answers.AnswerValue); break;
                   
                }
            }

            TableQuestion24 = function (Answers) {
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //regularly, where you or your family attend in case of illness?
                    case 277:
                        addRowQ24(RowNumber);
                        $("#Q24_DiseCareP_" + RowNumber).text(Answers.AnswerText);
                        if (RowNumber == 0) {
                            $("#Q24_Info_" + RowNumber).text("<%= GetLocalResourceObject("colab")%>");
                        }
                        else {
                            $("#Q24_Info_" + RowNumber).text($("#Q20_1_RWTI_" + RowNumber).text() + " , " + $("#Q20_1_Age_" + RowNumber).text());
                        }
                        break;
                    //Without being sick and for prevention, does this person see a doctor or nurse at least once a year?
                    case 278: $("#Q24_VisitMed_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //What is the main reason why this person does not consult the doctor?
                    case 279: $("#Q24_ReasonNotVisit_" + RowNumber).text(Answers.AnswerText); break;
                    //In the last 6 months, has this person had any health problem, illness or accident?
                    case 280: $("#Q24_HaveProblem_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Did this person have any consultation or medical attention for that illness or accident? Consider only those visits made to a professional doctor or nurse
                    case 281: $("#Q24_HaveAtentionMd_" + RowNumber).text(Answers.AnswerValue == "True" ? "<%= GetLocalResourceObject("yes")%>" : "<%= GetLocalResourceObject("not")%>"); break;
                    //Why did this person not have a consultation or attention?
                    case 282: $("#Q24_WhyNotMEdic_" + RowNumber).text(Answers.AnswerText); break;
                }
            }

            TableQuestion26 = function (Answers) {
                
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //Relationship with the interviewed
                    case 289:
                        addRowQ26(RowNumber);
                        $("#Q26_RWTI_" + RowNumber).text(Answers.AnswerText);
                        break;
                    //Disability
                    case 290: $("#Q26_Disability_" + RowNumber).text(Answers.AnswerText); break;


                }
            }

            TableQuestion28 = function (Answers) {
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //Relationship with the interviewed
                    case 297:
                        addRowQ28(RowNumber);
                        $("#Q28_Disease_" + RowNumber).text(Answers.AnswerText);
                        break;
                    //Disability
                    case 298: $("#Q28_Frenquency_" + RowNumber).text(Answers.AnswerText); break;

                }
            }

            TableQuestion41 = function (Answers) {
                var RowNumber = Answers.AnswerItem;
                switch (parseInt(Answers.QuestionCode)) {
                    //Relationship with the interviewed
                    case 335:
                        addRowQ41(RowNumber);
                        $("#Q41_Service_" + RowNumber).text(Answers.AnswerText);
                        break;
                    //Disability
                    case 336: $("#Q41_Availability_" + RowNumber).text(Answers.AnswerText); break;


                }
            }
        });


    </script>


    <script type="text/javascript">

        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
             $('#<%= grvEmployees.ClientID %> tbody tr').removeClass('info');
        }
        function IsRowSelected() {
            /// <summary>Validate if there is a selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                return true;
            }
            return false;
        }


        function pageLoad(sender, args) {
            /// <summary>Execute at load even at partial and ajax requests</summary>
            $('.btnAjaxAction').on('click', function () {
                var $this = $(this);
                $this.button('loading');
                setTimeout(function () { $this.button('reset'); }, 30000);
            });

            //In this section we initialize the popovers and tooltips for the elements.
            //We have both (popovers and tooltips) in order to accomplish information alert over desktop 
            //and mobiles devices
            InitializeTooltipsPopovers();

            //And the grvList selection row functionality
            $('#<%= grvEmployees.ClientID %>').on('click', 'tbody tr', function (event) {
                if (!$(this).hasClass('emptyRow')) {
                    $(this).toggleClass('info').siblings().removeClass('info');
                    $('#<%=hdfSelectedRowIndex.ClientID%>').val($(this).hasClass("info") ? $(this).index() : -1);

                    $( "#<%= lbtnAcceptSelectedEmployee.ClientID %>").focus();
                }
            });


            $('#<%= grvEmployees.ClientID %>').on('keypress', 'tbody tr', function (event) {
                $(this).click();
            });


            $('#<%= lbtnAcceptSelectedEmployee.ClientID%>').on('click', function (ev) {
                /// <summary>Handles the click event for button lbtnAcceptSelectedEmployee.</summary>
                ev.preventDefault();
                disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                disableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);

                if (IsRowSelected()) {
                    var employeCode = $('#<%= txtEmployeeCodeSearch.ClientID %>').val();
                    var obj = {
                        entity: { EmployeeCode: employeCode }, Lang: '<%= GetLocalResourceObject("Lang")%>', SurveyVersion: '<%= this.SurveyVersion %>'
                    };

                    employeeCodSelect = obj.entity.EmployeeCode;
                    employeeFullName = $(".fullNameGet:first").text();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json;charset=utf-8;",
                        url: "./SurveySummary.aspx/SurveySummaryGet",
                        data: JSON.stringify(obj),
                        // dataType: "json",
                        success: function (response) {
                            var result = response.d
                            datos = result;
                            if (result.error != 0) {
                                if (result.Survey != null) {
                                    if (result.Survey.SurveyStateCode == 2) {
                                        PersonalInfo(result.Survey);
                                        CompletedForm(result.SurveyAnswers);

                                        $('.btnAjaxAction').button('reset');
                                        $('#employeeSearchDialog').modal('hide');
                                    } else {

                                        MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("MsgEmployeeNoCompletedPeriod") %>', function () {
                                    $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                                    enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                                    enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                                    enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                                    $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                                });

                            }
                        } else {

                            MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("MsgEmployeeNoCompletedPeriod") %>', function () {
                                $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                                enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                            });
                        }
                    } else {
                        MostrarMensaje(TipoMensaje.ERROR, 'Error Internal: ' + result.Msg, function () {
                            $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                                enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                            });
                         }

                     }//end sucesss

                 });//end ajax
                               // __doPostBack('<%= lbtnAcceptSelectedEmployee.UniqueID %>', 'OnClick');
                }
                else {
                    MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidSelection") %>', function () {
                        $('#<%= lbtnAcceptSelectedEmployee.ClientID %>').button('reset');
                        enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                        enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                        enableButton($('#<%= btnEmployeeSearch.ClientID %>'));
                        $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                    });
                }


            });

            $('#<%=txtEmployeeCodeSearch.ClientID%>').on('keypress', function (event) {
                if (event.keyCode === 10 || event.keyCode === 13) {
                    var employeeSearchText = $.trim($('#<%= txtEmployeeCodeSearch.ClientID %>').val());
                    if (employeeSearchText.length > 0) {
                        SetControlValid($('#<%=txtEmployeeCodeSearch.ClientID%>').attr('id'));
                        $('#<%=btnEmployeeSearch.ClientID%>').click();
                    }
                    else {
                        SetControlInvalid($('#<%=txtEmployeeCodeSearch.ClientID%>').attr('id'));
                    }
                }
            });

            //each time a ajax or page load execue we need to sync the selected row with its value
            SetRowSelected();
        }

        function ShowEmployeeSearchDialog() {
            /// <summary>Show the Informed Consent Dialog</summary>                                    
            setTimeout(function () {
                $('#employeeSearchDialog').modal('show');
            }, 200);
        }

        function ProcessEmployeeSearchRequest(resetId) {
            /// <summary>Process the request for the employee search</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
            disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));

            var texnew = $('#<%= txtEmployeeCodeSearch.ClientID %>').val();
            var replanceText = texnew.replace(/<[^>]*>/g, '')
            $('#<%= txtEmployeeCodeSearch.ClientID %>').val(replanceText);

            if (!ValidateEmployeeSearch()) {

                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);


                MostrarMensaje(TipoMensaje.VALIDACION, '<%= GetLocalResourceObject("msgInvalidEntry") %>', function () {
                    setTimeout(function () {
                        ResetButton(resetId);
                        enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                        enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                        $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                    }, 200);
                });
                return false;
            }
            else {
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
                __doPostBack('<%= btnEmployeeSearch.UniqueID %>', '');
            }
            return false;
        }

        var setTAbIndex = function () {

            $("tbody tr").attr("tabIndex", function (i) {
                return 4;
            });

        };


        function ProcessEmployeeSearchResponse() {
            /// <summary>Process the response for the employee search</summary>
            setTimeout(function () {
                ResetButton($('#<%= btnEmployeeSearch.ClientID %>').id);
                enableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
                enableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
                $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', false);
                UnselectRow();
                setTAbIndex();
            }, 200);
        }

        function ProcessCancelEmployeeSearchRequest(resetId) {
            /// <summary>Handles the on client click event for button lbtnCancelEmployeeSearch.</summary>
            /// <param name="resetId" type="String">Id of the button to reset if not request proceed</param>
            disableButton($('#<%= lbtnAcceptSelectedEmployee.ClientID %>'));
            disableButton($('#<%= lbtnCancelEmployeeSearch.ClientID %>'));
            disableButton($('#<%= btnEmployeeSearch.ClientID %>'));
            $('#<%= txtEmployeeCodeSearch.ClientID %>').prop('disabled', true);
            return true;
        }

        function ValidateEmployeeSearch() {
            /// <summary>Validate the form with jquey validation plugin </summary>
            /// <returns> True if form is valid. False otherwise. </returns>
            $('#' + document.forms[0].id).validate().destroy();

            var validatorEmployeeSearch = null;

            if (validatorEmployeeSearch == null) {
                //declare the validator
                var validatorEmployeeSearch =
                    $('#' + document.forms[0].id).validate({
                        debug: true,
                        highlight: function (element, errorClass, validClass) {
                            SetControlInvalid($(element).attr('id'));
                        },
                        unhighlight: function (element, errorClass, validClass) {
                            SetControlValid($(element).attr('id'));
                        },
                        errorPlacement: function (error, element) { },
                        rules: {
                            <%= txtEmployeeCodeSearch.UniqueID %>: {
                    required: true
                        , minlength: 1
                            , normalizer: function(value) {


                                //var text = value.replace(/<[^>]*>/g, '');
                                return $.trim(String(value));
                            }
                }
            }
        });

            }

        var result = validatorEmployeeSearch.form();
        return result;
        }

        function SetRowSelected() {
            /// <summary>Set the class of the selected row</summary>  
            var selectedRowIndex = $('#<%=hdfSelectedRowIndex.ClientID%>').val();
            if (!isEmptyOrSpaces(selectedRowIndex) && selectedRowIndex != "-1") {
                var selectedRow = $('#<%= grvEmployees.ClientID %> tbody tr:eq(' + selectedRowIndex + ')');
                selectedRow.siblings().removeClass('info');
                if (!selectedRow.hasClass('info')) {
                    selectedRow.addClass('info');
                }
            }
        }

        function UnselectRow() {
            /// <summary>Unselect rows</summary>  
            $('#<%=hdfSelectedRowIndex.ClientID%>').val("-1");
            $('#<%= grvEmployees.ClientID %> tbody tr').removeClass('info');
        }
    </script>
    <style type="text/css" >
        .linea {
                border-top: 1px solid black;
                height: 2px;
                max-width: 100%;
                padding: 0;
                margin: 20px auto 0 auto;
        }
    </style>
</asp:Content>
