﻿using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetStudio
{
    public class Hash128
    {
        public byte[] bytes;

        public Hash128(EndianBinaryReader reader)
        {
            bytes = reader.ReadBytes(16);
        }
    }

    public class StructParameter
    {
        public MatrixParameter[] m_MatrixParams;
        public VectorParameter[] m_VectorParams;

        public StructParameter(EndianBinaryReader reader)
        {
            var m_NameIndex = reader.ReadInt32();
            var m_Index = reader.ReadInt32();
            var m_ArraySize = reader.ReadInt32();
            var m_StructSize = reader.ReadInt32();

            int numVectorParams = reader.ReadInt32();
            m_VectorParams = new VectorParameter[numVectorParams];
            for (int i = 0; i < numVectorParams; i++)
            {
                m_VectorParams[i] = new VectorParameter(reader);
            }

            int numMatrixParams = reader.ReadInt32();
            m_MatrixParams = new MatrixParameter[numMatrixParams];
            for (int i = 0; i < numMatrixParams; i++)
            {
                m_MatrixParams[i] = new MatrixParameter(reader);
            }
        }
    }

    public class SamplerParameter
    {
        public uint sampler;
        public int bindPoint;

        public SamplerParameter(EndianBinaryReader reader)
        {
            sampler = reader.ReadUInt32();
            bindPoint = reader.ReadInt32();
        }
    }
    public enum TextureDimension
    {
        Unknown = -1,
        None = 0,
        Any = 1,
        Tex2D = 2,
        Tex3D = 3,
        Cube = 4,
        Tex2DArray = 5,
        CubeArray = 6
    };

    public class SerializedTextureProperty
    {
        public string m_DefaultName;
        public TextureDimension m_TexDim;

        public SerializedTextureProperty(EndianBinaryReader reader)
        {
            m_DefaultName = reader.ReadAlignedString();
            m_TexDim = (TextureDimension)reader.ReadInt32();
        }
    }

    public enum SerializedPropertyType
    {
        Color = 0,
        Vector = 1,
        Float = 2,
        Range = 3,
        Texture = 4,
        Int = 5
    };

    public class SerializedProperty
    {
        public string m_Name;
        public string m_Description;
        public string[] m_Attributes;
        public SerializedPropertyType m_Type;
        public uint m_Flags;
        public float[] m_DefValue;
        public SerializedTextureProperty m_DefTexture;

        public SerializedProperty(EndianBinaryReader reader)
        {
            m_Name = reader.ReadAlignedString();
            m_Description = reader.ReadAlignedString();
            m_Attributes = reader.ReadStringArray();
            m_Type = (SerializedPropertyType)reader.ReadInt32();
            m_Flags = reader.ReadUInt32();
            m_DefValue = reader.ReadSingleArray(4);
            m_DefTexture = new SerializedTextureProperty(reader);
        }
    }

    public class SerializedProperties
    {
        public SerializedProperty[] m_Props;

        public SerializedProperties(EndianBinaryReader reader)
        {
            int numProps = reader.ReadInt32();
            m_Props = new SerializedProperty[numProps];
            for (int i = 0; i < numProps; i++)
            {
                m_Props[i] = new SerializedProperty(reader);
            }
        }
    }

    public class SerializedShaderFloatValue
    {
        public float val;
        public string name;

        public SerializedShaderFloatValue(EndianBinaryReader reader)
        {
            val = reader.ReadSingle();
            name = reader.ReadAlignedString();
        }
    }

    public class SerializedShaderRTBlendState
    {
        public SerializedShaderFloatValue srcBlend;
        public SerializedShaderFloatValue destBlend;
        public SerializedShaderFloatValue srcBlendAlpha;
        public SerializedShaderFloatValue destBlendAlpha;
        public SerializedShaderFloatValue blendOp;
        public SerializedShaderFloatValue blendOpAlpha;
        public SerializedShaderFloatValue colMask;

        public SerializedShaderRTBlendState(EndianBinaryReader reader)
        {
            srcBlend = new SerializedShaderFloatValue(reader);
            destBlend = new SerializedShaderFloatValue(reader);
            srcBlendAlpha = new SerializedShaderFloatValue(reader);
            destBlendAlpha = new SerializedShaderFloatValue(reader);
            blendOp = new SerializedShaderFloatValue(reader);
            blendOpAlpha = new SerializedShaderFloatValue(reader);
            colMask = new SerializedShaderFloatValue(reader);
        }
    }

    public class SerializedStencilOp
    {
        public SerializedShaderFloatValue pass;
        public SerializedShaderFloatValue fail;
        public SerializedShaderFloatValue zFail;
        public SerializedShaderFloatValue comp;

        public SerializedStencilOp(EndianBinaryReader reader)
        {
            pass = new SerializedShaderFloatValue(reader);
            fail = new SerializedShaderFloatValue(reader);
            zFail = new SerializedShaderFloatValue(reader);
            comp = new SerializedShaderFloatValue(reader);
        }
    }

    public class SerializedShaderVectorValue
    {
        public SerializedShaderFloatValue x;
        public SerializedShaderFloatValue y;
        public SerializedShaderFloatValue z;
        public SerializedShaderFloatValue w;
        public string name;

        public SerializedShaderVectorValue(EndianBinaryReader reader)
        {
            x = new SerializedShaderFloatValue(reader);
            y = new SerializedShaderFloatValue(reader);
            z = new SerializedShaderFloatValue(reader);
            w = new SerializedShaderFloatValue(reader);
            name = reader.ReadAlignedString();
        }
    }

    public enum FogMode
    {
        Unknown = -1,
        Disabled = 0,
        Linear = 1,
        Exp = 2,
        Exp2 = 3
    };

    public class SerializedShaderState
    {
        public string m_Name;
        public SerializedShaderRTBlendState[] rtBlend;
        public bool rtSeparateBlend;
        public SerializedShaderFloatValue zClip;
        public SerializedShaderFloatValue zTest;
        public SerializedShaderFloatValue zWrite;
        public SerializedShaderFloatValue culling;
        public SerializedShaderFloatValue conservative;
        public SerializedShaderFloatValue offsetFactor;
        public SerializedShaderFloatValue offsetUnits;
        public SerializedShaderFloatValue alphaToMask;
        public SerializedStencilOp stencilOp;
        public SerializedStencilOp stencilOpFront;
        public SerializedStencilOp stencilOpBack;
        public SerializedShaderFloatValue stencilReadMask;
        public SerializedShaderFloatValue stencilWriteMask;
        public SerializedShaderFloatValue stencilRef;
        public SerializedShaderFloatValue fogStart;
        public SerializedShaderFloatValue fogEnd;
        public SerializedShaderFloatValue fogDensity;
        public SerializedShaderVectorValue fogColor;
        public FogMode fogMode;
        public int gpuProgramID;
        public SerializedTagMap m_Tags;
        public int m_LOD;
        public bool lighting;

        public SerializedShaderState(ObjectReader reader)
        {
            var version = reader.version;

            m_Name = reader.ReadAlignedString();
            rtBlend = new SerializedShaderRTBlendState[8];
            for (int i = 0; i < 8; i++)
            {
                rtBlend[i] = new SerializedShaderRTBlendState(reader);
            }
            rtSeparateBlend = reader.ReadBoolean();
            reader.AlignStream();
            if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2)) //2017.2 and up
            {
                zClip = new SerializedShaderFloatValue(reader);
            }
            zTest = new SerializedShaderFloatValue(reader);
            zWrite = new SerializedShaderFloatValue(reader);
            culling = new SerializedShaderFloatValue(reader);
            if (version[0] >= 2020) //2020.1 and up
            {
                conservative = new SerializedShaderFloatValue(reader);
            }
            offsetFactor = new SerializedShaderFloatValue(reader);
            offsetUnits = new SerializedShaderFloatValue(reader);
            alphaToMask = new SerializedShaderFloatValue(reader);
            stencilOp = new SerializedStencilOp(reader);
            stencilOpFront = new SerializedStencilOp(reader);
            stencilOpBack = new SerializedStencilOp(reader);
            stencilReadMask = new SerializedShaderFloatValue(reader);
            stencilWriteMask = new SerializedShaderFloatValue(reader);
            stencilRef = new SerializedShaderFloatValue(reader);
            fogStart = new SerializedShaderFloatValue(reader);
            fogEnd = new SerializedShaderFloatValue(reader);
            fogDensity = new SerializedShaderFloatValue(reader);
            fogColor = new SerializedShaderVectorValue(reader);
            fogMode = (FogMode)reader.ReadInt32();
            gpuProgramID = reader.ReadInt32();
            m_Tags = new SerializedTagMap(reader);
            m_LOD = reader.ReadInt32();
            lighting = reader.ReadBoolean();
            reader.AlignStream();
        }
    }

    public class ShaderBindChannel
    {
        public sbyte source;
        public sbyte target;

        public ShaderBindChannel(EndianBinaryReader reader)
        {
            source = reader.ReadSByte();
            target = reader.ReadSByte();
        }
    }

    public class ParserBindChannels
    {
        public ShaderBindChannel[] m_Channels;
        public uint m_SourceMap;

        public ParserBindChannels(EndianBinaryReader reader)
        {
            int numChannels = reader.ReadInt32();
            m_Channels = new ShaderBindChannel[numChannels];
            for (int i = 0; i < numChannels; i++)
            {
                m_Channels[i] = new ShaderBindChannel(reader);
            }
            reader.AlignStream();

            m_SourceMap = reader.ReadUInt32();
        }
    }

    public class VectorParameter
    {
        public int m_NameIndex;
        public int m_Index;
        public int m_ArraySize;
        public sbyte m_Type;
        public sbyte m_Dim;

        public VectorParameter(EndianBinaryReader reader)
        {
            m_NameIndex = reader.ReadInt32();
            m_Index = reader.ReadInt32();
            m_ArraySize = reader.ReadInt32();
            m_Type = reader.ReadSByte();
            m_Dim = reader.ReadSByte();
            reader.AlignStream();
        }
    }

    public class MatrixParameter
    {
        public int m_NameIndex;
        public int m_Index;
        public int m_ArraySize;
        public sbyte m_Type;
        public sbyte m_RowCount;

        public MatrixParameter(EndianBinaryReader reader)
        {
            m_NameIndex = reader.ReadInt32();
            m_Index = reader.ReadInt32();
            m_ArraySize = reader.ReadInt32();
            m_Type = reader.ReadSByte();
            m_RowCount = reader.ReadSByte();
            reader.AlignStream();
        }
    }

    public class TextureParameter
    {
        public int m_NameIndex;
        public int m_Index;
        public int m_SamplerIndex;
        public sbyte m_Dim;

        public TextureParameter(ObjectReader reader)
        {
            var version = reader.version;

            m_NameIndex = reader.ReadInt32();
            m_Index = reader.ReadInt32();
            m_SamplerIndex = reader.ReadInt32();
            if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 3)) //2017.3 and up
            {
                var m_MultiSampled = reader.ReadBoolean();
            }
            m_Dim = reader.ReadSByte();
            reader.AlignStream();
        }
    }

    public class BufferBinding
    {
        public int m_NameIndex;
        public int m_Index;
        public int m_ArraySize;

        public BufferBinding(ObjectReader reader)
        {
            var version = reader.version;

            m_NameIndex = reader.ReadInt32();
            m_Index = reader.ReadInt32();
            if (version[0] >= 2020) //2020.1 and up
            {
                m_ArraySize = reader.ReadInt32();
            }
        }
    }

    public class ConstantBuffer
    {
        public int m_NameIndex;
        public MatrixParameter[] m_MatrixParams;
        public VectorParameter[] m_VectorParams;
        public StructParameter[] m_StructParams;
        public int m_Size;
        public bool m_IsPartialCB;

        public ConstantBuffer(ObjectReader reader)
        {
            var version = reader.version;

            m_NameIndex = reader.ReadInt32();

            int numMatrixParams = reader.ReadInt32();
            m_MatrixParams = new MatrixParameter[numMatrixParams];
            for (int i = 0; i < numMatrixParams; i++)
            {
                m_MatrixParams[i] = new MatrixParameter(reader);
            }

            int numVectorParams = reader.ReadInt32();
            m_VectorParams = new VectorParameter[numVectorParams];
            for (int i = 0; i < numVectorParams; i++)
            {
                m_VectorParams[i] = new VectorParameter(reader);
            }
            if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 3)) //2017.3 and up
            {
                int numStructParams = reader.ReadInt32();
                m_StructParams = new StructParameter[numStructParams];
                for (int i = 0; i < numStructParams; i++)
                {
                    m_StructParams[i] = new StructParameter(reader);
                }
            }
            m_Size = reader.ReadInt32();

            if ((version[0] == 2020 && version[1] > 3) ||
               (version[0] == 2020 && version[1] == 3 && version[2] >= 2) || //2020.3.2f1 and up
               (version[0] > 2021) ||
               (version[0] == 2021 && version[1] > 1) ||
               (version[0] == 2021 && version[1] == 1 && version[2] >= 4)) //2021.1.4f1 and up
            {
                m_IsPartialCB = reader.ReadBoolean();
                reader.AlignStream();
            }
        }
    }

    public class UAVParameter
    {
        public int m_NameIndex;
        public int m_Index;
        public int m_OriginalIndex;

        public UAVParameter(EndianBinaryReader reader)
        {
            m_NameIndex = reader.ReadInt32();
            m_Index = reader.ReadInt32();
            m_OriginalIndex = reader.ReadInt32();
        }
    }

    public enum ShaderGpuProgramType
    {
        Unknown = 0,
        GLLegacy = 1,
        GLES31AEP = 2,
        GLES31 = 3,
        GLES3 = 4,
        GLES = 5,
        GLCore32 = 6,
        GLCore41 = 7,
        GLCore43 = 8,
        DX9VertexSM20 = 9,
        DX9VertexSM30 = 10,
        DX9PixelSM20 = 11,
        DX9PixelSM30 = 12,
        DX10Level9Vertex = 13,
        DX10Level9Pixel = 14,
        DX11VertexSM40 = 15,
        DX11VertexSM50 = 16,
        DX11PixelSM40 = 17,
        DX11PixelSM50 = 18,
        DX11GeometrySM40 = 19,
        DX11GeometrySM50 = 20,
        DX11HullSM50 = 21,
        DX11DomainSM50 = 22,
        MetalVS = 23,
        MetalFS = 24,
        SPIRV = 25,
        ConsoleVS = 26,
        ConsoleFS = 27,
        ConsoleHS = 28,
        ConsoleDS = 29,
        ConsoleGS = 30,
        RayTracing = 31,
        PS5NGGC = 32
    };

    public class SerializedProgramParameters
    {
        public VectorParameter[] m_VectorParams;
        public MatrixParameter[] m_MatrixParams;
        public TextureParameter[] m_TextureParams;
        public BufferBinding[] m_BufferParams;
        public ConstantBuffer[] m_ConstantBuffers;
        public BufferBinding[] m_ConstantBufferBindings;
        public UAVParameter[] m_UAVParams;
        public SamplerParameter[] m_Samplers;

        public SerializedProgramParameters(ObjectReader reader)
        {
            int numVectorParams = reader.ReadInt32();
            m_VectorParams = new VectorParameter[numVectorParams];
            for (int i = 0; i < numVectorParams; i++)
            {
                m_VectorParams[i] = new VectorParameter(reader);
            }

            int numMatrixParams = reader.ReadInt32();
            m_MatrixParams = new MatrixParameter[numMatrixParams];
            for (int i = 0; i < numMatrixParams; i++)
            {
                m_MatrixParams[i] = new MatrixParameter(reader);
            }

            int numTextureParams = reader.ReadInt32();
            m_TextureParams = new TextureParameter[numTextureParams];
            for (int i = 0; i < numTextureParams; i++)
            {
                m_TextureParams[i] = new TextureParameter(reader);
            }

            int numBufferParams = reader.ReadInt32();
            m_BufferParams = new BufferBinding[numBufferParams];
            for (int i = 0; i < numBufferParams; i++)
            {
                m_BufferParams[i] = new BufferBinding(reader);
            }

            int numConstantBuffers = reader.ReadInt32();
            m_ConstantBuffers = new ConstantBuffer[numConstantBuffers];
            for (int i = 0; i < numConstantBuffers; i++)
            {
                m_ConstantBuffers[i] = new ConstantBuffer(reader);
            }

            int numConstantBufferBindings = reader.ReadInt32();
            m_ConstantBufferBindings = new BufferBinding[numConstantBufferBindings];
            for (int i = 0; i < numConstantBufferBindings; i++)
            {
                m_ConstantBufferBindings[i] = new BufferBinding(reader);
            }

            int numUAVParams = reader.ReadInt32();
            m_UAVParams = new UAVParameter[numUAVParams];
            for (int i = 0; i < numUAVParams; i++)
            {
                m_UAVParams[i] = new UAVParameter(reader);
            }

            int numSamplers = reader.ReadInt32();
            m_Samplers = new SamplerParameter[numSamplers];
            for (int i = 0; i < numSamplers; i++)
            {
                m_Samplers[i] = new SamplerParameter(reader);
            }
        }
    }

    public class SerializedSubProgram
    {
        public uint m_BlobIndex;
        public ParserBindChannels m_Channels;
        public ushort[] m_KeywordIndices;
        public sbyte m_ShaderHardwareTier;
        public ShaderGpuProgramType m_GpuProgramType;
        public SerializedProgramParameters m_Parameters;
        public VectorParameter[] m_VectorParams;
        public MatrixParameter[] m_MatrixParams;
        public TextureParameter[] m_TextureParams;
        public BufferBinding[] m_BufferParams;
        public ConstantBuffer[] m_ConstantBuffers;
        public BufferBinding[] m_ConstantBufferBindings;
        public UAVParameter[] m_UAVParams;
        public SamplerParameter[] m_Samplers;

        private static bool HasGlobalLocalKeywordIndices(ObjectReader reader) => reader.objInfo.serializedType.Match("E99740711222CD922E9A6F92FF1EB07A") || reader.objInfo.serializedType.Match("450A058C218DAF000647948F2F59DA6D");
        private static bool HasInstancedStructuredBuffers(ObjectReader reader) => reader.objInfo.serializedType.Match("E99740711222CD922E9A6F92FF1EB07A");

        public SerializedSubProgram(ObjectReader reader)
        {
            var version = reader.version;

            m_BlobIndex = reader.ReadUInt32();
            m_Channels = new ParserBindChannels(reader);

            if ((version[0] >= 2019 && version[0] < 2021) || (version[0] == 2021 && version[1] < 2) || HasGlobalLocalKeywordIndices(reader)) //2019 ~2021.1
            {
                var m_GlobalKeywordIndices = reader.ReadUInt16Array();
                reader.AlignStream();
                var m_LocalKeywordIndices = reader.ReadUInt16Array();
                reader.AlignStream();
            }
            else
            {
                m_KeywordIndices = reader.ReadUInt16Array();
                if (version[0] >= 2017) //2017 and up
                {
                    reader.AlignStream();
                }
            }

            m_ShaderHardwareTier = reader.ReadSByte();
            m_GpuProgramType = (ShaderGpuProgramType)reader.ReadSByte();
            reader.AlignStream();

            if ((version[0] == 2020 && version[1] > 3) ||
               (version[0] == 2020 && version[1] == 3 && version[2] >= 2) || //2020.3.2f1 and up
               (version[0] > 2021) ||
               (version[0] == 2021 && version[1] > 1) ||
               (version[0] == 2021 && version[1] == 1 && version[2] >= 1)) //2021.1.1f1 and up
            {
                m_Parameters = new SerializedProgramParameters(reader);
            }
            else
            {
                int numVectorParams = reader.ReadInt32();
                m_VectorParams = new VectorParameter[numVectorParams];
                for (int i = 0; i < numVectorParams; i++)
                {
                    m_VectorParams[i] = new VectorParameter(reader);
                }

                int numMatrixParams = reader.ReadInt32();
                m_MatrixParams = new MatrixParameter[numMatrixParams];
                for (int i = 0; i < numMatrixParams; i++)
                {
                    m_MatrixParams[i] = new MatrixParameter(reader);
                }

                int numTextureParams = reader.ReadInt32();
                m_TextureParams = new TextureParameter[numTextureParams];
                for (int i = 0; i < numTextureParams; i++)
                {
                    m_TextureParams[i] = new TextureParameter(reader);
                }

                int numBufferParams = reader.ReadInt32();
                m_BufferParams = new BufferBinding[numBufferParams];
                for (int i = 0; i < numBufferParams; i++)
                {
                    m_BufferParams[i] = new BufferBinding(reader);
                }

                int numConstantBuffers = reader.ReadInt32();
                m_ConstantBuffers = new ConstantBuffer[numConstantBuffers];
                for (int i = 0; i < numConstantBuffers; i++)
                {
                    m_ConstantBuffers[i] = new ConstantBuffer(reader);
                }

                int numConstantBufferBindings = reader.ReadInt32();
                m_ConstantBufferBindings = new BufferBinding[numConstantBufferBindings];
                for (int i = 0; i < numConstantBufferBindings; i++)
                {
                    m_ConstantBufferBindings[i] = new BufferBinding(reader);
                }

                int numUAVParams = reader.ReadInt32();
                m_UAVParams = new UAVParameter[numUAVParams];
                for (int i = 0; i < numUAVParams; i++)
                {
                    m_UAVParams[i] = new UAVParameter(reader);
                }

                if (version[0] >= 2017) //2017 and up
                {
                    int numSamplers = reader.ReadInt32();
                    m_Samplers = new SamplerParameter[numSamplers];
                    for (int i = 0; i < numSamplers; i++)
                    {
                        m_Samplers[i] = new SamplerParameter(reader);
                    }
                }
            }

            if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2)) //2017.2 and up
            {
                if (version[0] >= 2021) //2021.1 and up
                {
                    var m_ShaderRequirements = reader.ReadInt64();
                }
                else
                {
                    var m_ShaderRequirements = reader.ReadInt32();
                }
            }

            if (HasInstancedStructuredBuffers(reader))
            {
                int numInstancedStructuredBuffers = reader.ReadInt32();
                var m_InstancedStructuredBuffers = new ConstantBuffer[numInstancedStructuredBuffers];
                for (int i = 0; i < numInstancedStructuredBuffers; i++)
                {
                    m_InstancedStructuredBuffers[i] = new ConstantBuffer(reader);
                }
            }
        }
    }

    public class SerializedProgram
    {
        public SerializedSubProgram[] m_SubPrograms;
        public SerializedProgramParameters m_CommonParameters;
        public ushort[] m_SerializedKeywordStateMask;

        public SerializedProgram(ObjectReader reader)
        {
            var version = reader.version;

            int numSubPrograms = reader.ReadInt32();
            m_SubPrograms = new SerializedSubProgram[numSubPrograms];
            for (int i = 0; i < numSubPrograms; i++)
            {
                m_SubPrograms[i] = new SerializedSubProgram(reader);
            }

            if ((version[0] == 2020 && version[1] > 3) ||
               (version[0] == 2020 && version[1] == 3 && version[2] >= 2) || //2020.3.2f1 and up
               (version[0] > 2021) ||
               (version[0] == 2021 && version[1] > 1) ||
               (version[0] == 2021 && version[1] == 1 && version[2] >= 1)) //2021.1.1f1 and up
            {
                m_CommonParameters = new SerializedProgramParameters(reader);
            }

            if (version[0] > 2022 || (version[0] == 2022 && version[1] >= 1)) //2022.1 and up
            {
                m_SerializedKeywordStateMask = reader.ReadUInt16Array();
                reader.AlignStream();
            }
        }
    }

    public enum PassType
    {
        Normal = 0,
        Use = 1,
        Grab = 2
    };

    public class SerializedPass
    {
        public Hash128[] m_EditorDataHash;
        public byte[] m_Platforms;
        public ushort[] m_LocalKeywordMask;
        public ushort[] m_GlobalKeywordMask;
        public KeyValuePair<string, int>[] m_NameIndices;
        public PassType m_Type;
        public SerializedShaderState m_State;
        public uint m_ProgramMask;
        public SerializedProgram progVertex;
        public SerializedProgram progFragment;
        public SerializedProgram progGeometry;
        public SerializedProgram progHull;
        public SerializedProgram progDomain;
        public SerializedProgram progRayTracing;
        public bool m_HasInstancingVariant;
        public string m_UseName;
        public string m_Name;
        public string m_TextureName;
        public SerializedTagMap m_Tags;
        public ushort[] m_SerializedKeywordStateMask;

        public SerializedPass(ObjectReader reader)
        {
            var version = reader.version;

            if (version[0] > 2020 || (version[0] == 2020 && version[1] >= 2)) //2020.2 and up
            {
                int numEditorDataHash = reader.ReadInt32();
                m_EditorDataHash = new Hash128[numEditorDataHash];
                for (int i = 0; i < numEditorDataHash; i++)
                {
                    m_EditorDataHash[i] = new Hash128(reader);
                }
                reader.AlignStream();
                m_Platforms = reader.ReadUInt8Array();
                reader.AlignStream();
                if (version[0] < 2021 || (version[0] == 2021 && version[1] < 2)) //2021.1 and down
                {
                    m_LocalKeywordMask = reader.ReadUInt16Array();
                    reader.AlignStream();
                    m_GlobalKeywordMask = reader.ReadUInt16Array();
                    reader.AlignStream();
                }
            }

            int numIndices = reader.ReadInt32();
            m_NameIndices = new KeyValuePair<string, int>[numIndices];
            for (int i = 0; i < numIndices; i++)
            {
                m_NameIndices[i] = new KeyValuePair<string, int>(reader.ReadAlignedString(), reader.ReadInt32());
            }

            m_Type = (PassType)reader.ReadInt32();
            m_State = new SerializedShaderState(reader);
            m_ProgramMask = reader.ReadUInt32();
            progVertex = new SerializedProgram(reader);
            progFragment = new SerializedProgram(reader);
            progGeometry = new SerializedProgram(reader);
            progHull = new SerializedProgram(reader);
            progDomain = new SerializedProgram(reader);
            if (version[0] > 2019 || (version[0] == 2019 && version[1] >= 3)) //2019.3 and up
            {
                progRayTracing = new SerializedProgram(reader);
            }
            m_HasInstancingVariant = reader.ReadBoolean();
            if (version[0] >= 2018) //2018 and up
            {
                var m_HasProceduralInstancingVariant = reader.ReadBoolean();
            }
            reader.AlignStream();
            m_UseName = reader.ReadAlignedString();
            m_Name = reader.ReadAlignedString();
            m_TextureName = reader.ReadAlignedString();
            m_Tags = new SerializedTagMap(reader);
            if (version[0] == 2021 && version[1] >= 2) //2021.2 ~2021.x
            {
                m_SerializedKeywordStateMask = reader.ReadUInt16Array();
                reader.AlignStream();
            }
        }
    }

    public class SerializedTagMap
    {
        public KeyValuePair<string, string>[] tags;

        public SerializedTagMap(EndianBinaryReader reader)
        {
            int numTags = reader.ReadInt32();
            tags = new KeyValuePair<string, string>[numTags];
            for (int i = 0; i < numTags; i++)
            {
                tags[i] = new KeyValuePair<string, string>(reader.ReadAlignedString(), reader.ReadAlignedString());
            }
        }
    }

    public class SerializedSubShader
    {
        public SerializedPass[] m_Passes;
        public SerializedTagMap m_Tags;
        public int m_LOD;

        public SerializedSubShader(ObjectReader reader)
        {
            int numPasses = reader.ReadInt32();
            m_Passes = new SerializedPass[numPasses];
            for (int i = 0; i < numPasses; i++)
            {
                m_Passes[i] = new SerializedPass(reader);
            }

            m_Tags = new SerializedTagMap(reader);
            m_LOD = reader.ReadInt32();
        }
    }

    public class SerializedShaderDependency
    {
        public string from;
        public string to;

        public SerializedShaderDependency(EndianBinaryReader reader)
        {
            from = reader.ReadAlignedString();
            to = reader.ReadAlignedString();
        }
    }

    public class SerializedCustomEditorForRenderPipeline
    {
        public string customEditorName;
        public string renderPipelineType;

        public SerializedCustomEditorForRenderPipeline(EndianBinaryReader reader)
        {
            customEditorName = reader.ReadAlignedString();
            renderPipelineType = reader.ReadAlignedString();
        }
    }

    public class SerializedShader
    {
        public SerializedProperties m_PropInfo;
        public SerializedSubShader[] m_SubShaders;
        public string[] m_KeywordNames;
        public byte[] m_KeywordFlags;
        public string m_Name;
        public string m_CustomEditorName;
        public string m_FallbackName;
        public SerializedShaderDependency[] m_Dependencies;
        public SerializedCustomEditorForRenderPipeline[] m_CustomEditorForRenderPipelines;
        public bool m_DisableNoSubshadersMessage;

        public SerializedShader(ObjectReader reader)
        {
            var version = reader.version;

            m_PropInfo = new SerializedProperties(reader);

            int numSubShaders = reader.ReadInt32();
            m_SubShaders = new SerializedSubShader[numSubShaders];
            for (int i = 0; i < numSubShaders; i++)
            {
                m_SubShaders[i] = new SerializedSubShader(reader);
            }

            if (version[0] > 2021 || (version[0] == 2021 && version[1] >= 2)) //2021.2 and up
            {
                m_KeywordNames = reader.ReadStringArray();
                m_KeywordFlags = reader.ReadUInt8Array();
                reader.AlignStream();
            }

            m_Name = reader.ReadAlignedString();
            m_CustomEditorName = reader.ReadAlignedString();
            m_FallbackName = reader.ReadAlignedString();

            int numDependencies = reader.ReadInt32();
            m_Dependencies = new SerializedShaderDependency[numDependencies];
            for (int i = 0; i < numDependencies; i++)
            {
                m_Dependencies[i] = new SerializedShaderDependency(reader);
            }

            if (version[0] >= 2021) //2021.1 and up
            {
                int m_CustomEditorForRenderPipelinesSize = reader.ReadInt32();
                m_CustomEditorForRenderPipelines = new SerializedCustomEditorForRenderPipeline[m_CustomEditorForRenderPipelinesSize];
                for (int i = 0; i < m_CustomEditorForRenderPipelinesSize; i++)
                {
                    m_CustomEditorForRenderPipelines[i] = new SerializedCustomEditorForRenderPipeline(reader);
                }
            }

            m_DisableNoSubshadersMessage = reader.ReadBoolean();
            reader.AlignStream();
        }
    }

    public enum ShaderCompilerPlatform
    {
        None = -1,
        GL = 0,
        D3D9 = 1,
        Xbox360 = 2,
        PS3 = 3,
        D3D11 = 4,
        GLES20 = 5,
        NaCl = 6,
        Flash = 7,
        D3D11_9x = 8,
        GLES3Plus = 9,
        PSP2 = 10,
        PS4 = 11,
        XboxOne = 12,
        PSM = 13,
        Metal = 14,
        OpenGLCore = 15,
        N3DS = 16,
        WiiU = 17,
        Vulkan = 18,
        Switch = 19,
        XboxOneD3D12 = 20,
        GameCoreXboxOne = 21,
        GameCoreScarlett = 22,
        PS5 = 23,
        PS5NGGC = 24
    };

    public class Shader : NamedObject
    {
        public static bool Parsable;

        public byte[] m_Script;
        //5.3 - 5.4
        public uint decompressedSize;
        public byte[] m_SubProgramBlob;
        //5.5 and up
        public SerializedShader m_ParsedForm;
        public ShaderCompilerPlatform[] platforms;
        public uint[][] offsets;
        public uint[][] compressedLengths;
        public uint[][] decompressedLengths;
        public byte[] compressedBlob;

        public Shader(ObjectReader reader) : base(reader)
        {
            if (version[0] == 5 && version[1] >= 5 || version[0] > 5) //5.5 and up
            {
                m_ParsedForm = new SerializedShader(reader);
                platforms = reader.ReadUInt32Array().Select(x => (ShaderCompilerPlatform)x).ToArray();
                if (version[0] > 2019 || (version[0] == 2019 && version[1] >= 3)) //2019.3 and up
                {
                    offsets = reader.ReadUInt32ArrayArray();
                    compressedLengths = reader.ReadUInt32ArrayArray();
                    decompressedLengths = reader.ReadUInt32ArrayArray();
                }
                else
                {
                    offsets = reader.ReadUInt32Array().Select(x => new[] { x }).ToArray();
                    compressedLengths = reader.ReadUInt32Array().Select(x => new[] { x }).ToArray();
                    decompressedLengths = reader.ReadUInt32Array().Select(x => new[] { x }).ToArray();
                }
                compressedBlob = reader.ReadUInt8Array();
                reader.AlignStream();
                if (reader.Game.Type.IsGISubGroup())
                {
                    if (BinaryPrimitives.ReadInt32LittleEndian(compressedBlob) == -1)
                    {
                        compressedBlob = reader.ReadUInt8Array(); //blobDataBlocks
                        reader.AlignStream();
                    }
                }

                var m_DependenciesCount = reader.ReadInt32();
                for (int i = 0; i < m_DependenciesCount; i++)
                {
                    new PPtr<Shader>(reader);
                }

                if (version[0] >= 2018)
                {
                    var m_NonModifiableTexturesCount = reader.ReadInt32();
                    for (int i = 0; i < m_NonModifiableTexturesCount; i++)
                    {
                        var first = reader.ReadAlignedString();
                        new PPtr<Texture>(reader);
                    }
                }

                var m_ShaderIsBaked = reader.ReadBoolean();
                reader.AlignStream();
            }
            else
            {
                m_Script = reader.ReadUInt8Array();
                reader.AlignStream();
                var m_PathName = reader.ReadAlignedString();
                if (version[0] == 5 && version[1] >= 3) //5.3 - 5.4
                {
                    decompressedSize = reader.ReadUInt32();
                    m_SubProgramBlob = reader.ReadUInt8Array();
                }
            }
        }
    }
}
