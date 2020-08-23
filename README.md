# Potato raytracing

Potato is a simple raytracing C# program, developed during free time.

The orignal goal was to create a dll and export it into a software to render images easly.
Since, my new goal is still create a dll but, running on gpu in realtime.

:warning: Because mipmap is not supported, changing default cubemap will result to a pixelize background

**Still many things to improve, to refactor and to simply for better performance.**

**How to use it**

Simply clone and run it.
Modify 'option.xml' in the 'Resources' folder to play with settings.

# New Features!

  - Cubemap
  - Reflection
  - Multi-threading (improved!)
  - Video
# Features

 -  Sphere rendering
 -  Texture on sphere
 -  Mesh rendering (only obj)
 -  Mutiple lights
 -  Double precision
 -  Custom scene file
 -  Multi-threading
# Screenshots
![multiple lights](https://raw.githubusercontent.com/BaboucheOne/PotatoRaytracing/master/renderedImages/27_10_19_image1.bmp)

![multiple lights and textures](https://raw.githubusercontent.com/BaboucheOne/PotatoRaytracing/master/renderedImages/04_11_19_image1.bmp)

![cubemap and light](https://raw.githubusercontent.com/BaboucheOne/PotatoRaytracing/master/renderedImages/22_05_20_cubemap_1.bmp)

Reflection image will come soon. (But you can render your own image!)

See also [rendered images folder](https://github.com/BaboucheOne/PotatoRaytracing/tree/master/renderedImages) for more!

# Futher features

 - K-Tree
 - 2 type of rendering (Solid color or cubemap)
 - Textures on meshs
 - Mimap for cubemap
 - Material on object

# Documentation

 - [Scratchapixel 2.0 - Ray-Tracing Rendering Technique](https://www.scratchapixel.com/lessons/3d-basic-rendering/ray-tracing-overview/ray-tracing-rendering-technique-overview)
 - [Write raytracer in rust part 2 - Light](https://bheisler.github.io/post/writing-raytracer-in-rust-part-2/)
 - [Custom raytracer renderer](https://dietertack.files.wordpress.com/2017/11/tackdieter_paper.pdf)
 - [Free obj models](http://casual-effects.com/data/index.html)
