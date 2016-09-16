# Unity 3D metaballs
Unity 3D implementation of marching cubes algorithm for rendering metaballs. It can be used for gameplay purposes (gels in Valve's Portal 2), or as a decoration, as shown in the image below.

![](https://github.com/dario-zubovic/metaballs/raw/master/gif.gif)

Metaball functions are envaluated on GPU or CPU, but GPU mode performs about 10x faster. Currently there is only 1 falloff functions available:

```f(d) = r^2 / d^2```

Where *r* is radius of the metaball, and *d* is distance from metaball's center to current point. Additional functions can be quickly added as a new compute shader based on the existing one.

References:
* William E. Lorensen and Harvey E. Cline. 1987. Marching cubes: A high resolution 3D surface construction algorithm. In Proceedings of the 14th annual conference on Computer graphics and interactive techniques (SIGGRAPH '87), Maureen C. Stone (Ed.). ACM, New York, NY, USA, 163-169.
* Brian Wyvill and Geoff Wyvill. Field functions for implicit surfaces. The Visual Computer, 5:75–82, December 1989.