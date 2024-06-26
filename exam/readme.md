## Exam Project: Adaptive 2D integrator

### Implementation:

The algorithm is implemented in int2.cs, compiled into the library int2.dll.

The signature is:

	static (double,double,int) integ2D(
			Func<double,double,double> f,
			double a, double b,
			Func<double,double> d,
			Func<double,double> u,
			double acc = 0.001,
			double eps = 0.001,
			int max_nfev = 9999){ ... }

The function returns a tuple containing (integral estimate, error estimate, number of function evaluations).
It throws an OperationCanceledException if the maximum number of function evaluations is reached.

The function works by first defining a function:

![equation](https://latex.codecogs.com/svg.image?&space;F(x)=\int_{d(x)}^{u(x)}f(x,y)dy)

Which evaluates the integral using the adaptive integrator from the integration homework part C.
This ensures that we can also submit functions which take the value double.Infinity.
The requested absolute error passed to this integration is rescaled relative to the total requested error: 

![equation](https://latex.codecogs.com/svg.image?\delta\to\delta/\sqrt{b-a}).

This is needed since the total variance in the inner integral is (roughly) scaled up by the length of the outer integration.

The function F is then passed to the same adaptive integrator as before. This is also the only part that calculates the estimated error. This is quite an informal way to estimate the error since it seems the error is only accounted for along 1 dimension. However, we we see in the next section that the error estimates seem to be reliable.

### Testing:

To test the implementation we evaluate the four (easily verified) integrals:

1:
![equation](https://latex.codecogs.com/svg.image?\int_{-1}^{1}\int_{-\sqrt{1-x^2}}^{\sqrt{1-x^2}}\sqrt{1-x^2-y^2}dy&space;dx=\frac{2\pi}{3})

2:
![equation](https://latex.codecogs.com/svg.image?\int_{1}^{3}\int_{0}^{\frac{\pi}{x}}\sin(xy)dydx=2\ln3&space;)

3:
![equation](https://latex.codecogs.com/svg.image?\int_{-\infty}^{\infty}\int_{1}^{\frac{1}{x^2}}\frac{e^{-x^2}}{y^2}dydx=\sqrt{\pi})

4:
![equation](https://latex.codecogs.com/svg.image?\int_{-\infty}^{\infty}\int_{-\infty}^{\infty}e^{-(x^2&plus;y^2)}dydx=\pi)

The results are written in the file: Test\_Result.txt

### Extra:

The laplace transform is defined as:

![equation](https://latex.codecogs.com/svg.image?F(s)=\int_0^{\infty}f(t)e^{-ts}dt)

The convolution between two functions f,g is:

![equation](https://latex.codecogs.com/svg.image?f*g(t)=\int&space;_0^t&space;f(u)g(t-u)du&space;)

The convolution theorem states that:

![equation](https://latex.codecogs.com/svg.image?\mathcal{L}(f*g)(s)=F(s)\cdot&space;G(s))

We can check this theorem by numerically evaluating the resulting double integral:

![equation](https://latex.codecogs.com/svg.image?F(s)G(s)=\int_{0}^{\infty}\int_{0}^{t}f(u)g(t-u)e^{-st}dudt)

In the plot Out.convolution.svg the numerical and analytical results are plotted for. f(t) = te^-t, g(t) = sin(t).
### Evaluation:

I rate myself 9/10. Subtracting one point for the superficial error calculation.

