namespace Assets.Src.Debug.Cheats
{
    using UnityEngine;

    public abstract class Performable
    {
        public abstract void Perform();

        public string Name { get; protected set; }
    }

    public class ShaderPerformable : Performable
    {
        private Renderer renderer;

        private Shader shader;

        public ShaderPerformable(Renderer renderer, Shader shader)
        {
            this.renderer = renderer;
            this.shader = shader;
            Name = "Apply Shader " + shader.name;
        }

        public override void Perform()
        {
            renderer.sharedMaterial.shader = shader;
        }
    }

    public class TexturePerformable : Performable
    {
        private Renderer renderer;

        private Texture texture;

        public TexturePerformable(Renderer renderer, Texture texture)
        {
            this.renderer = renderer;
            this.texture = texture;
            Name = "Apply Texture " + texture.name;
        }

        public override void Perform()
        {
            renderer.sharedMaterial.mainTexture = texture;
        }
    }

    public class MaterialPerformable : Performable
    {
        private Renderer renderer;

        private Material material;

        public MaterialPerformable(Renderer renderer, Material material, string name = null)
        {
            this.renderer = renderer;
            this.material = material;
            Name = name ?? "Apply Material " + material.name;
        }

        public override void Perform()
        {
            renderer.sharedMaterial = this.material;
        }
    }

    public class FirstPerformable : Performable
    {
        public FirstPerformable()
        {
            Name = "Mr First [Cheat Stub]";
        }

        public override void Perform()
        {
            Debug.Log("Perform First");
            Debugger.Log("Perform First ");
        }
    }

    public class SeconfPerformable : Performable
    {
        public SeconfPerformable()
        {
            Name = "Mr Second [Cheat Stub]";
        }

        public override void Perform()
        {
            Debug.Log("Perform Second");
            Debugger.Log("Perform Second");
        }
    }
}