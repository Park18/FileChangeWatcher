// FuzzyShannonStaticLib.cpp : 정적 라이브러리를 위한 함수를 정의합니다.
//
#pragma warning(disable:4996)
#include "pch.h"
#include "framework.h"
#include "FuzzyShannonStaticLib.h"
#include <map>
#include <iostream>
#include <string>
#include <cstdio>
#include <limits>


#define FFUZZYPP_DECLARATIONS
#define _CRT_NONSTDC_NO_WARNINGS


#include "ffuzzy.hpp"
using namespace ffuzzy;

// 파일 크기 최적화
#define OPTIMIZE_CONSTANT_FILE_SIZE


typedef union
{
	digest_unorm_t u;
	digest_t d;
} unified_digest_t;
typedef union
{
	digest_long_unorm_t u;
	digest_long_t d;
} unified_digest_long_t;

double LibFuzzyShannon::Shannon(const char* filepath)
{
	FILE* fp = NULL;
	int fsize = 0;
	char* read_buf = NULL;
	int binary_num = 20;

	fopen_s(&fp, filepath, "rb");
	if (fp == NULL)
	{
		return 10;
	}
	else
	{
		fseek(fp, 0, SEEK_END);
		fsize = ftell(fp);
		if (fsize > binary_num)
		{
			fsize = binary_num;
		}
		fseek(fp, 0, SEEK_SET);

		read_buf = (char*)calloc(fsize + 1, sizeof(char));
		fread(read_buf, sizeof(char), fsize, fp);
		fclose(fp);

		unsigned char* p1 = (unsigned char*)read_buf;

		std::map<char, int> frequencies;

		int numlen = fsize;

		for (int i = 0; i < numlen; i++)
		{
			frequencies[(unsigned int)*(p1 + i)] ++;

		}


		double infocontent = 0;

		for (std::pair<char, int> p : frequencies) {
			double freq = static_cast<double>(p.second) / numlen;
			infocontent -= freq * log(freq) / log(2);
		}
		//std::cout << infocontent << std::endl;
		return infocontent;
	}
}

std::string LibFuzzyShannon::computehash(const char* filepath)
{
	digest_generator gen;
	digest_unorm_t d;
	char digestbuf[digest_unorm_t::max_natural_chars];
	static const size_t BUFFER_SIZE = 4096;
	unsigned char filebuf[BUFFER_SIZE];


	static_assert(digest_unorm_t::max_natural_width <= std::numeric_limits<unsigned>::max(),
		"digest_unorm_t::max_natural_width must be small enough.");
	char digestformatbuf[digest_unorm_t::max_natural_width_digits + 9];
	sprintf(digestformatbuf, "%%-%us  %%s\n",
		static_cast<unsigned>(digest_unorm_t::max_natural_width));
#if 0
	fprintf(stderr, "FORMAT STRING: %s\n", digestformatbuf);
#endif

	// iterate over all files given

	FILE* fp = NULL;

	fopen_s(&fp, filepath, "rb");

	// error when failed to open file
	if (!fp)
	{
		perror(filepath);
		return "open error";
	}

#ifdef OPTIMIZE_CONSTANT_FILE_SIZE

	bool seekable = false;
	off_t filesize;
	if (fseek(fp, 0, SEEK_END) == 0)
	{
		seekable = true;
		filesize = ftell(fp);
		if (fseek(fp, 0, SEEK_SET) != 0)
		{
			fprintf(stderr, "%s: could not seek to the beginning.\n", filepath);
			fclose(fp);
			return "open error";
		}

		if (!gen.set_file_size_constant(filesize))
		{
			fprintf(stderr, "%s: cannot optimize performance for this file.\n", filepath);
			fclose(fp);
			return "open error";
		}
	}
	else
	{
#if 0
		fprintf(stderr, "%s: seek operation is not available.\n", filename);
#endif
	}
#endif

	// update generator by given file stream and buffer
	if (!gen.update_by_stream<BUFFER_SIZE>(fp, filebuf))
	{
		fprintf(stderr, "%s: failed to update fuzzy hashes.\n", filepath);
		fclose(fp);
		return "open error";
	}
	fclose(fp);

	if (!gen.copy_digest(d))
	{
		if (gen.is_total_size_clamped())
			fprintf(stderr, "%s: too big to process.\n", filepath);
#ifdef OPTIMIZE_CONSTANT_FILE_SIZE
		else if (seekable && (digest_filesize_t(filesize) != gen.total_size()))
			fprintf(stderr, "%s: file size changed while reading (or arithmetic overflow?)\n", filepath);
#endif
		else
			fprintf(stderr, "%s: failed to copy digest with unknown error.\n", filepath);
		return "open error";
	}

	if (!d.pretty_unsafe(digestbuf))
	{
		fprintf(stderr, "%s: failed to stringize the digest.\n", filepath);
		return "open error";
	}
	//std::cout << digestbuf << std::endl;


	return digestbuf;
}

int LibFuzzyShannon::comparehash(std::string hash1, std::string hash2)
{
	const char* first_hash = hash1.c_str();
	const char* second_hash = hash2.c_str();

#if 1
	unified_digest_t h1, h2;
#else
	unified_digest_long_t h1, h2;
#endif
	char digestbuf[decltype(h1.u)::max_natural_chars];

	// Parse digests
	if (!decltype(h1.u)::parse(h1.u, first_hash))
	{
		fprintf(stderr, "error: failed to parse HASH1.\n");
		return 101;
	}
	if (!decltype(h2.u)::parse(h2.u, second_hash))
	{
		fprintf(stderr, "error: failed to parse HASH2.\n");
		return 101;
	}

	/*
		Restringize digests (just for demo)
		Notice that we're using h1.d instead of h1.u?
		This is not a good example but works perfectly.
	*/
	if (!h1.d.pretty_unsafe(digestbuf))
	{
		fprintf(stderr, "abort: failed to re-stringize HASH1.\n");
		return 101;
	}
	//printf("HASH1 : %s\n", digestbuf);
	if (!h2.d.pretty_unsafe(digestbuf))
	{
		fprintf(stderr, "abort: failed to re-stringize HASH2.\n");
		return 101;
	}
	//printf("HASH2 : %s\n", digestbuf);

	// Normalize digests and restringize them
	decltype(h1.d)::normalize(h1.d, h1.u);
	decltype(h2.d)::normalize(h2.d, h2.u);
	if (!h1.d.pretty_unsafe(digestbuf))
	{
		fprintf(stderr, "abort: failed to re-stringize HASH1.\n");
		return 101;
	}
	//printf("NORM1 : %s\n", digestbuf);
	if (!h2.d.pretty_unsafe(digestbuf))
	{
		fprintf(stderr, "abort: failed to re-stringize HASH2.\n");
		return 101;
	}
	//printf("NORM2 : %s\n", digestbuf);

	/*
		Compare them
		"Unnormalized" form has compare function but slow because of additional normalization)

		Note:
		Use `compare` or `compare<comparison_version::latest>` for latest
		version and `compare<comparison_version::v2_9>` for version 2.9 emulation.
	*/
	digest_comparison_score_t score =
		decltype(h1.d)::compare<comparison_version::v2_9>(h1.d, h2.d);
	//printf("SCORE: %u\n", unsigned(score)); // safe to cast to unsigned (value is in [0,100])

	return unsigned(score);
}